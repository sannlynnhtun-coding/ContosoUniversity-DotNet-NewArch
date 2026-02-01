using System;
using System.Threading.Tasks;
using ContosoUniversity.Domain.Features.Departments;
using ContosoUniversity.Domain.Features.Instructors;
using Shouldly;
using Xunit;

namespace ContosoUniversity.IntegrationTests.Pages.Departments;

[Collection(nameof(SliceFixture))]
public class DetailsTests : SliceTestBase
{
    public DetailsTests(SliceFixture fixture) : base(fixture) { }

    [Fact]
    public async Task Should_get_department_details()
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

        var result = await Fixture.ExecuteServiceAsync<IDepartmentService, DepartmentDetailDto>(s => 
            s.GetDepartmentAsync(dept.Id));

        result.ShouldNotBeNull();
        result.Name.ShouldBe(dept.Name);
        result.AdministratorName.ShouldBe(admin.FullName);
    }
}