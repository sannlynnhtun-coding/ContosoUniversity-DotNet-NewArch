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
public class DeleteTests : SliceTestBase
{
    public DeleteTests(SliceFixture fixture) : base(fixture) { }

    [Fact]
    public async Task Should_delete_department()
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

        await Fixture.ExecuteServiceAsync<IDepartmentService>(s => s.DeleteDepartmentAsync(dept.Id));

        var any = await Fixture.ExecuteDbContextAsync(db => db.Departments.Where(d => d.Id == dept.Id).AnyAsync());

        any.ShouldBeFalse();
    }
}