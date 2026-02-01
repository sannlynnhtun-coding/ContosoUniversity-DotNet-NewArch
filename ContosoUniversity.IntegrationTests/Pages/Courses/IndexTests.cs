using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Domain.Features.Courses;
using ContosoUniversity.Domain.Features.Departments;
using ContosoUniversity.Domain.Features.Instructors;
using Shouldly;
using Xunit;

namespace ContosoUniversity.IntegrationTests.Pages.Courses;

[Collection(nameof(SliceFixture))]
public class IndexTests : SliceTestBase
{
    public IndexTests(SliceFixture fixture) : base(fixture) { }

    [Fact]
    public async Task Should_return_all_courses()
    {
        var admin = new Instructor
        {
            FirstMidName = "George",
            LastName = "Jones",
            HireDate = DateTime.Today
        };
        await Fixture.InsertAsync(admin);

        var englishDept = new Department
        {
            Name = "English",
            InstructorId = admin.Id,
            Budget = 123m,
            StartDate = DateTime.Today
        };
        var historyDept = new Department
        {
            Name = "History",
            InstructorId = admin.Id,
            Budget = 123m,
            StartDate = DateTime.Today
        };

        var english = new Course
        {
            Credits = 4,
            Department = englishDept,
            Id = Fixture.NextCourseNumber(),
            Title = "English 101"
        };
        var history = new Course
        {
            Credits = 4,
            Department = historyDept,
            Id = Fixture.NextCourseNumber(),
            Title = "History 101"
        };
        await Fixture.InsertAsync(
            englishDept, 
            historyDept, 
            english, 
            history);

        var result = await Fixture.ExecuteServiceAsync<ICourseService, List<CourseListDto>>(s => 
            s.GetCoursesAsync());

        result.ShouldNotBeNull();
        result.Count.ShouldBeGreaterThanOrEqualTo(2);

        var courseIds = result.Select(c => c.Id).ToList();
        courseIds.ShouldContain(english.Id);
        courseIds.ShouldContain(history.Id);
    }
}