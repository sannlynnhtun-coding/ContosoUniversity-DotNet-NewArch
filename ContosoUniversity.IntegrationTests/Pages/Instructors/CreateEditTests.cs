using System;
using System.Collections.Generic;
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
public class CreateEditTests : SliceTestBase
{
    public CreateEditTests(SliceFixture fixture) : base(fixture) { }

    [Fact]
    public async Task Should_create_new_instructor()
    {
        var suffix = Guid.NewGuid().ToString("N")[..8];
        var lastName = "Seinfeld_" + suffix;
        var englishDept = new Department
        {
            Name = "English_" + suffix,
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

        var dto = new InstructorEditDto
        {
            FirstMidName = "Jerry",
            LastName = lastName,
            HireDate = DateTime.Today,
            OfficeAssignmentLocation = "Houston",
            SelectedCourses = new List<int> { english101.Id, english201.Id }
        };

        await Fixture.ExecuteServiceAsync<IInstructorService>(s => s.CreateInstructorAsync(dto));

        var created = await Fixture.ExecuteDbContextAsync(db => db.Instructors
            .Where(i => i.LastName == lastName && i.FirstMidName == "Jerry")
            .Include(i => i.CourseAssignments)
            .Include(i => i.OfficeAssignment)
            .SingleOrDefaultAsync());

        created.ShouldNotBeNull();
        created.FirstMidName.ShouldBe(dto.FirstMidName);
        created.LastName.ShouldBe(dto.LastName);
        created.HireDate.ShouldBe(dto.HireDate);
        created.OfficeAssignment.ShouldNotBeNull();
        created.OfficeAssignment.Location.ShouldBe(dto.OfficeAssignmentLocation);
        created.CourseAssignments.Count.ShouldBe(2);
    }

    [Fact]
    public async Task Should_edit_instructor_details()
    {
        var instructor = new Instructor
        {
            FirstMidName = "George",
            LastName = "Costanza",
            HireDate = DateTime.Today
        };
        await Fixture.InsertAsync(instructor);

        var dto = new InstructorEditDto
        {
            Id = instructor.Id,
            FirstMidName = "Jerry",
            LastName = "Seinfeld",
            HireDate = DateTime.Today,
            OfficeAssignmentLocation = "Houston",
            SelectedCourses = new List<int>()
        };

        await Fixture.ExecuteServiceAsync<IInstructorService>(s => s.UpdateInstructorAsync(dto));

        var edited = await Fixture.ExecuteDbContextAsync(db => db.Instructors
            .Where(i => i.Id == instructor.Id)
            .Include(i => i.CourseAssignments)
            .Include(i => i.OfficeAssignment)
            .SingleOrDefaultAsync());

        edited.FirstMidName.ShouldBe(dto.FirstMidName);
        edited.LastName.ShouldBe(dto.LastName);
        edited.HireDate.ShouldBe(dto.HireDate);
        edited.OfficeAssignment.ShouldNotBeNull();
        edited.OfficeAssignment.Location.ShouldBe(dto.OfficeAssignmentLocation);
    }

    [Fact]
    public async Task Should_merge_course_instructors()
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

        var instructor = new Instructor
        {
            FirstMidName = "George",
            LastName = "Costanza",
            HireDate = DateTime.Today
        };
        await Fixture.InsertAsync(instructor);
        await Fixture.InsertAsync(new CourseAssignment { CourseId = english101.Id, InstructorId = instructor.Id });

        var dto = new InstructorEditDto
        {
            Id = instructor.Id,
            FirstMidName = "Jerry",
            LastName = "Seinfeld",
            HireDate = DateTime.Today,
            OfficeAssignmentLocation = "Houston",
            SelectedCourses = new List<int> { english201.Id }
        };

        await Fixture.ExecuteServiceAsync<IInstructorService>(s => s.UpdateInstructorAsync(dto));

        var edited = await Fixture.ExecuteDbContextAsync(db => db.Instructors
            .Where(i => i.Id == instructor.Id)
            .Include(i => i.CourseAssignments)
            .Include(i => i.OfficeAssignment)
            .SingleOrDefaultAsync());

        edited.FirstMidName.ShouldBe(dto.FirstMidName);
        edited.LastName.ShouldBe(dto.LastName);
        edited.HireDate.ShouldBe(dto.HireDate);
        edited.OfficeAssignment.ShouldNotBeNull();
        edited.OfficeAssignment.Location.ShouldBe(dto.OfficeAssignmentLocation);
        edited.CourseAssignments.Count.ShouldBe(1);
        edited.CourseAssignments.First().CourseId.ShouldBe(english201.Id);
    }
}