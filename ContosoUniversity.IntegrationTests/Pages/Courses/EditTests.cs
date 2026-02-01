using System;
using System.Threading.Tasks;
using ContosoUniversity.Domain.Features.Courses;
using ContosoUniversity.Domain.Features.Departments;
using ContosoUniversity.Domain.Features.Instructors;
using Shouldly;
using Xunit;

namespace ContosoUniversity.IntegrationTests.Pages.Courses;

[Collection(nameof(SliceFixture))]
public class EditTests : SliceTestBase
{
    public EditTests(SliceFixture fixture) : base(fixture) { }

    [Fact]
    public async Task Should_query_for_command()
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

    [Fact]
    public async Task Should_edit()
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
        var newDept = new Department
        {
            Name = "English",
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
        await Fixture.InsertAsync(dept, newDept, course);

        var dto = new CourseEditDto
        {
            Id = course.Id,
            Credits = 5,
            Title = "English 202",
            DepartmentId = newDept.Id
        };

        await Fixture.ExecuteServiceAsync<ICourseService>(s => s.UpdateCourseAsync(dto));

        var edited = await Fixture.FindAsync<Course>(course.Id);

        edited.ShouldNotBeNull();
        edited.DepartmentId.ShouldBe(newDept.Id);
        edited.Credits.ShouldBe(dto.Credits);
        edited.Title.ShouldBe(dto.Title);
    }
}