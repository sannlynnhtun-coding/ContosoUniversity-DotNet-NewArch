using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Domain.Features.Courses;
using ContosoUniversity.Domain.Features.Departments;
using ContosoUniversity.Domain.Features.Enrollments;
using ContosoUniversity.Domain.Features.Instructors;
using ContosoUniversity.Domain.Features.Students;
using Shouldly;
using Xunit;

namespace ContosoUniversity.IntegrationTests.Pages.Instructors;

[Collection(nameof(SliceFixture))]
public class IndexTests : SliceTestBase
{
    public IndexTests(SliceFixture fixture) : base(fixture) { }

    [Fact]
    public async Task Should_get_list_instructor_with_details()
    {
        var englishDept = new Department
        {
            Name = "English",
            StartDate = DateTime.Today
        };
        await Fixture.InsertAsync(englishDept);

        var english101 = new Course
        {
            DepartmentId = englishDept.Id,
            Title = "English 101",
            Credits = 4,
            Id = Fixture.NextCourseNumber()
        };
        var english201 = new Course
        {
            DepartmentId = englishDept.Id,
            Title = "English 201",
            Credits = 4,
            Id = Fixture.NextCourseNumber()
        };

        await Fixture.InsertAsync(english101, english201);

        var admin = new Instructor
        {
            FirstMidName = "George",
            LastName = "Costanza",
            HireDate = DateTime.Today
        };
        await Fixture.InsertAsync(admin);
        
        // Add course assignments manually since we don't have a direct service for that in the test fixture yet
        await Fixture.InsertAsync(
            new CourseAssignment { CourseId = english101.Id, InstructorId = admin.Id },
            new CourseAssignment { CourseId = english201.Id, InstructorId = admin.Id }
        );

        var instructor2 = new Instructor
        {
            FirstMidName = "Jerry",
            LastName = "Seinfeld",
            HireDate = DateTime.Today
        };
        await Fixture.InsertAsync(instructor2);

        var student1 = new Student
        {
            FirstMidName = "Cosmo",
            LastName = "Kramer",
            EnrollmentDate = DateTime.Today
        };
        var student2 = new Student
        {
            FirstMidName = "Elaine",
            LastName = "Benes",
            EnrollmentDate = DateTime.Today
        };

        await Fixture.InsertAsync(student1, student2);

        var enrollment1 = new Enrollment { StudentId = student1.Id, CourseId = english101.Id };
        var enrollment2 = new Enrollment { StudentId = student2.Id, CourseId = english101.Id };

        await Fixture.InsertAsync(enrollment1, enrollment2);

        // Test Instructors
        var instructors = await Fixture.ExecuteServiceAsync<IInstructorService, List<InstructorListDto>>(s => 
            s.GetInstructorsAsync());

        instructors.ShouldNotBeNull();
        instructors.Count.ShouldBeGreaterThanOrEqualTo(2);
        instructors.Select(i => i.Id).ShouldContain(admin.Id);
        instructors.Select(i => i.Id).ShouldContain(instructor2.Id);

        // Test Courses for Selected Instructor
        var selectedInstructor = instructors.FirstOrDefault(i => i.Id == admin.Id);
        selectedInstructor.ShouldNotBeNull();
        selectedInstructor.CourseAssignments.Count().ShouldBe(2);

        // Test Enrollments for Selected Course
        var enrollments = await Fixture.ExecuteServiceAsync<IEnrollmentService, List<EnrollmentListDto>>(s => 
            s.GetEnrollmentsForCourseAsync(english101.Id));

        enrollments.ShouldNotBeNull();
        enrollments.Count.ShouldBe(2);
    }
}