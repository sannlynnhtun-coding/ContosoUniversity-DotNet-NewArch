using System;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Domain.Features.Courses;
using ContosoUniversity.Domain.Features.Departments;
using ContosoUniversity.Domain.Features.Instructors;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;

namespace ContosoUniversity.IntegrationTests.Pages.Courses;

[Collection(nameof(SliceFixture))]
public class CreateTests : SliceTestBase
{
    public CreateTests(SliceFixture fixture) : base(fixture) { }

    [Fact]
    public async Task Should_create_new_course()
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

        var dto = new CourseEditDto
        {
            Credits = 4,
            DepartmentId = dept.Id,
            Id = Fixture.NextCourseNumber(),
            Title = "English 101"
        };

        await Fixture.ExecuteServiceAsync<ICourseService>(s => s.CreateCourseAsync(dto));

        var created = await Fixture.ExecuteDbContextAsync(db => db.Courses
            .Where(c => c.Id == dto.Id)
            .SingleOrDefaultAsync());

        created.ShouldNotBeNull();
        created.DepartmentId.ShouldBe(dept.Id);
        created.Credits.ShouldBe(dto.Credits);
        created.Title.ShouldBe(dto.Title);
    }
}