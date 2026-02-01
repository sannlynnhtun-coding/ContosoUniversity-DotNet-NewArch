using System;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Domain.Features.Departments;
using ContosoUniversity.Domain.Features.Instructors;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;

namespace ContosoUniversity.IntegrationTests.Pages.Departments;

[Collection(nameof(SliceFixture))]
public class EditTests : SliceTestBase
{
    public EditTests(SliceFixture fixture) : base(fixture) { }

    [Fact]
    public async Task Should_get_edit_department_details()
    {
        var admin = new Instructor
        {
            FirstMidName = "George",
            LastName = "Costanza",
            HireDate = DateTime.Today
        };
        await Fixture.InsertAsync(admin);

        var dept = new Department
        {
            Name = "History",
            InstructorId = admin.Id,
            Budget = 123m,
            StartDate = DateTime.Today
        };
        await Fixture.InsertAsync(dept);

        var result = await Fixture.ExecuteServiceAsync<IDepartmentService, DepartmentEditDto>(s => 
            s.GetDepartmentForEditAsync(dept.Id));

        result.ShouldNotBeNull();
        result.Name.ShouldBe(dept.Name);
        result.InstructorId.ShouldBe(admin.Id);
    }

    [Fact]
    public async Task Should_edit_department()
    {
        var admin1 = new Instructor
        {
            FirstMidName = "George",
            LastName = "Costanza",
            HireDate = DateTime.Today
        };
        var admin2 = new Instructor
        {
            FirstMidName = "Jerry",
            LastName = "Seinfeld",
            HireDate = DateTime.Today
        };
        await Fixture.InsertAsync(admin1, admin2);

        var dept = new Department
        {
            Name = "History",
            InstructorId = admin1.Id,
            Budget = 123m,
            StartDate = DateTime.Today
        };
        await Fixture.InsertAsync(dept);

        var dto = new DepartmentEditDto
        {
            Id = dept.Id,
            Name = "English",
            InstructorId = admin2.Id,
            StartDate = DateTime.Today.AddDays(-1),
            Budget = 456m,
            RowVersion = dept.RowVersion
        };

        await Fixture.ExecuteServiceAsync<IDepartmentService>(s => s.UpdateDepartmentAsync(dto));

        var result = await Fixture.ExecuteDbContextAsync(db => db.Departments
            .Where(d => d.Id == dept.Id)
            .Include(d => d.Administrator)
            .SingleOrDefaultAsync());

        result.Name.ShouldBe(dto.Name);
        result.Administrator.Id.ShouldBe(admin2.Id);
        result.StartDate.ShouldBe(dto.StartDate);
        result.Budget.ShouldBe(dto.Budget);
    }
}