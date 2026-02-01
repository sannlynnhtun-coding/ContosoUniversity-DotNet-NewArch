using System;
using System.Threading.Tasks;
using ContosoUniversity.Domain.Features.Courses;
using ContosoUniversity.Domain.Features.Departments;
using ContosoUniversity.Domain.Features.Instructors;
using Shouldly;
using Xunit;

namespace ContosoUniversity.IntegrationTests.Pages.Courses;

[Collection(nameof(SliceFixture))]
public class DetailsTests : SliceTestBase
{
    public DetailsTests(SliceFixture fixture) : base(fixture) { }

    [Fact]
    public async Task Should_query_for_details()
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

        var course = new Course
        {
            Credits = 4,
            Department = dept,
            Id = Fixture.NextCourseNumber(),
            Title = "English 101"
        };

        await Fixture.InsertAsync(dept, course);

        var result = await Fixture.ExecuteServiceAsync<ICourseService, CourseDetailDto>(s => 
            s.GetCourseAsync(course.Id));

        result.ShouldNotBeNull();
        result.Credits.ShouldBe(course.Credits);
        result.DepartmentName.ShouldBe(dept.Name);
        result.Title.ShouldBe(course.Title);
    }
}