using System;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Domain.Features.Students;
using ContosoUniversity.Domain.Shared;
using Shouldly;
using Xunit;

namespace ContosoUniversity.IntegrationTests.Pages.Students;

[Collection(nameof(SliceFixture))]
public class IndexTests : SliceTestBase
{
    public IndexTests(SliceFixture fixture) : base(fixture) { }

    [Fact]
    public async Task Should_return_all_items_for_default_search()
    {
        var suffix = DateTime.Now.Ticks.ToString();
        var lastName = "Schmoe" + suffix;
        var student1 = new Student
        {
            EnrollmentDate = DateTime.Today,
            FirstMidName = "Joe",
            LastName = lastName
        };
        var student2 = new Student
        {
            EnrollmentDate = DateTime.Today,
            FirstMidName = "Jane",
            LastName = lastName
        };
        await Fixture.InsertAsync(student1, student2);

        var result = await Fixture.ExecuteServiceAsync<IStudentService, PaginatedList<StudentListDto>>(s => 
            s.GetStudentsAsync(null, lastName, 1, 10));

        result.Count.ShouldBeGreaterThanOrEqualTo(2);
        result.Select(r => r.Id).ShouldContain(student1.Id);
        result.Select(r => r.Id).ShouldContain(student2.Id);
    }

    [Fact]
    public async Task Should_sort_based_on_name()
    {
        var suffix = DateTime.Now.Ticks.ToString();
        var lastName = "Schmoe" + suffix;
        var student1 = new Student
        {
            EnrollmentDate = DateTime.Today,
            FirstMidName = "Joe",
            LastName = lastName + "zzz"
        };
        var student2 = new Student
        {
            EnrollmentDate = DateTime.Today,
            FirstMidName = "Jane",
            LastName = lastName + "aaa"
        };
        await Fixture.InsertAsync(student1, student2);

        var result = await Fixture.ExecuteServiceAsync<IStudentService, PaginatedList<StudentListDto>>(s => 
            s.GetStudentsAsync("name_desc", lastName, 1, 10));

        result.Count.ShouldBe(2);
        result[0].Id.ShouldBe(student1.Id);
        result[1].Id.ShouldBe(student2.Id);
    }
}