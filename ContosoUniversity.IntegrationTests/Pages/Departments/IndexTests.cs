using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Domain.Features.Departments;
using ContosoUniversity.Domain.Features.Instructors;
using Shouldly;
using Xunit;

namespace ContosoUniversity.IntegrationTests.Pages.Departments;

[Collection(nameof(SliceFixture))]
public class IndexTests : SliceTestBase
{
    public IndexTests(SliceFixture fixture) : base(fixture) { }

    [Fact]
    public async Task Should_list_departments()
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
        var dept2 = new Department
        {
            Name = "English",
            InstructorId = admin.Id,
            Budget = 456m,
            StartDate = DateTime.Today
        };

        await Fixture.InsertAsync(dept, dept2);

        var result = await Fixture.ExecuteServiceAsync<IDepartmentService, List<DepartmentListDto>>(s => 
            s.GetDepartmentsAsync());

        result.ShouldNotBeNull();
        result.Count.ShouldBeGreaterThanOrEqualTo(2);
        result.Select(m => m.Id).ShouldContain(dept.Id);
        result.Select(m => m.Id).ShouldContain(dept2.Id);
    }
}