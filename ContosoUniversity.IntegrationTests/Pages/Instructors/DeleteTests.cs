using System;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Domain.Features.Courses;
using ContosoUniversity.Domain.Features.Departments;
using ContosoUniversity.Domain.Features.Instructors;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;

namespace ContosoUniversity.IntegrationTests.Pages.Instructors;

[Collection(nameof(SliceFixture))]
public class DeleteTests : SliceTestBase
{
    public DeleteTests(SliceFixture fixture) : base(fixture) { }

    [Fact]
    public async Task Should_query_for_command()
    {
        var instructor = new Instructor
        {
            FirstMidName = "George",
            LastName = "Costanza",
            HireDate = DateTime.Today
        };
        await Fixture.InsertAsync(instructor);

        var result = await Fixture.ExecuteServiceAsync<IInstructorService, InstructorDetailDto>(s => 
            s.GetInstructorAsync(instructor.Id));

        result.ShouldNotBeNull();
        result.FirstMidName.ShouldBe(instructor.FirstMidName);
    }

    [Fact]
    public async Task Should_delete_instructor()
    {
        var instructor = new Instructor
        {
            FirstMidName = "George",
            LastName = "Costanza",
            HireDate = DateTime.Today
        };
        await Fixture.InsertAsync(instructor);

        var englishDept = new Department
        {
            Name = "English",
            StartDate = DateTime.Today,
            InstructorId = instructor.Id
        };
        await Fixture.InsertAsync(englishDept);

        var english101 = new Course
        {
            DepartmentId = englishDept.Id,
            Title = "English 101",
            Credits = 4,
            Id = Fixture.NextCourseNumber()
        };
        await Fixture.InsertAsync(english101);

        await Fixture.InsertAsync(new CourseAssignment { CourseId = english101.Id, InstructorId = instructor.Id });

        await Fixture.ExecuteServiceAsync<IInstructorService>(s => s.DeleteInstructorAsync(instructor.Id));

        var instructorCount = await Fixture.ExecuteDbContextAsync(db => db.Instructors
            .Where(i => i.Id == instructor.Id)
            .CountAsync());

        instructorCount.ShouldBe(0);

        var dbDept = await Fixture.ExecuteDbContextAsync(db => db.Departments.FindAsync(englishDept.Id));
        dbDept.InstructorId.ShouldBeNull();

        var courseInstructorCount = await Fixture.ExecuteDbContextAsync(db => db.CourseAssignments
            .Where(ci => ci.InstructorId == instructor.Id)
            .CountAsync());

        courseInstructorCount.ShouldBe(0);
    }
}