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
public class CreateTests : SliceTestBase
{
    public CreateTests(SliceFixture fixture) : base(fixture) { }

    [Fact]
    public async Task Should_create_new_department()
    {
        var suffix = Guid.NewGuid().ToString("N")[..8];
        var departmentName = "Engineering_" + suffix;
        var admin = new Instructor
        {
            FirstMidName = "George",
            LastName = "Costanza",
            HireDate = DateTime.Today
        };
        await Fixture.InsertAsync(admin);

        var dto = new DepartmentEditDto
        {
            Budget = 10m,
            Name = departmentName,
            StartDate = DateTime.Now.Date,
            InstructorId = admin.Id
        };

        await Fixture.ExecuteServiceAsync<IDepartmentService>(s => s.CreateDepartmentAsync(dto));

        var created = await Fixture.ExecuteDbContextAsync(db => db.Departments
            .Where(d => d.Name == departmentName)
            .SingleOrDefaultAsync());

        created.ShouldNotBeNull();
        created.Budget.ShouldBe(dto.Budget);
        created.StartDate.ShouldBe(dto.StartDate);
        created.InstructorId.ShouldBe(admin.Id);
    }
}