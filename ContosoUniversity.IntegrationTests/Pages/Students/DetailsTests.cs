using System;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Domain.Features.Courses;
using ContosoUniversity.Domain.Features.Departments;
using ContosoUniversity.Domain.Features.Enrollments;
using ContosoUniversity.Domain.Features.Instructors;
using ContosoUniversity.Domain.Features.Students;
using Shouldly;
using Xunit;

namespace ContosoUniversity.IntegrationTests.Pages.Students;

[Collection(nameof(SliceFixture))]
public class DetailsTests : SliceTestBase
{
    public DetailsTests(SliceFixture fixture) : base(fixture) { }

    [Fact]
    public async Task Should_get_details()
    {
        var admin = new Instructor
        {
            FirstMidName = "George",
            LastName = "Costanza",
            HireDate = DateTime.Today
        };
        await Fixture.InsertAsync(admin);

        var englishDept = new Department
        {
            InstructorId = admin.Id,
            Budget = 123m,
            Name = "English 101",
            StartDate = DateTime.Today
        };
        await Fixture.InsertAsync(englishDept);

        var course1 = new Course
        {
            DepartmentId = englishDept.Id,
            Credits = 10,
            Id = Fixture.NextCourseNumber(),
            Title = "Course 1"
        };
        var course2 = new Course
        {
            DepartmentId = englishDept.Id,
            Credits = 10,
            Id = Fixture.NextCourseNumber(),
            Title = "Course 2"
        };
        await Fixture.InsertAsync(course1, course2);

        var student = new Student
        {
            FirstMidName = "Joe",
            LastName = "Schmoe",
            EnrollmentDate = new DateTime(2013, 1, 1)
        };
        await Fixture.InsertAsync(student);

        var enrollment1 = new Enrollment
        {
            CourseId = course1.Id,
            Grade = Grade.A,
            StudentId = student.Id
        };
        var enrollment2 = new Enrollment
        {
            CourseId = course2.Id,
            Grade = Grade.F,
            StudentId = student.Id
        };
        await Fixture.InsertAsync(enrollment1, enrollment2);

        var details = await Fixture.ExecuteServiceAsync<IStudentService, StudentDetailDto>(s => 
            s.GetStudentAsync(student.Id));

        details.ShouldNotBeNull();
        details.FirstMidName.ShouldBe(student.FirstMidName);
        details.LastName.ShouldBe(student.LastName);
        details.EnrollmentDate.ShouldBe(student.EnrollmentDate);
        details.Enrollments.Count().ShouldBe(2);
    }
}