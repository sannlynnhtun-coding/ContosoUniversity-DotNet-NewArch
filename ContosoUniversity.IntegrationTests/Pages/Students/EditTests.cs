using System;
using System.Threading.Tasks;
using ContosoUniversity.Domain.Features.Students;
using Shouldly;
using Xunit;

namespace ContosoUniversity.IntegrationTests.Pages.Students;

[Collection(nameof(SliceFixture))]
public class EditTests : SliceTestBase
{
    public EditTests(SliceFixture fixture) : base(fixture) { }

    [Fact]
    public async Task Should_get_edit_details()
    {
        var student = new Student
        {
            FirstMidName = "Joe",
            LastName = "Schmoe",
            EnrollmentDate = DateTime.Today
        };
        await Fixture.InsertAsync(student);

        var result = await Fixture.ExecuteServiceAsync<IStudentService, StudentEditDto>(s => 
            s.GetStudentForEditAsync(student.Id));

        result.FirstMidName.ShouldBe(student.FirstMidName);
        result.LastName.ShouldBe(student.LastName);
        result.EnrollmentDate.ShouldBe(student.EnrollmentDate);
    }

    [Fact]
    public async Task Should_edit_student()
    {
        var student = new Student
        {
            FirstMidName = "Joe",
            LastName = "Schmoe",
            EnrollmentDate = DateTime.Today
        };
        await Fixture.InsertAsync(student);

        var dto = new StudentEditDto
        {
            Id = student.Id,
            FirstMidName = "Mary",
            LastName = "Smith",
            EnrollmentDate = DateTime.Today.AddYears(-1)
        };

        await Fixture.ExecuteServiceAsync<IStudentService>(s => s.UpdateStudentAsync(dto));

        var dbStudent = await Fixture.FindAsync<Student>(student.Id);

        dbStudent.ShouldNotBeNull();
        dbStudent.FirstMidName.ShouldBe(dto.FirstMidName);
        dbStudent.LastName.ShouldBe(dto.LastName);
        dbStudent.EnrollmentDate.ShouldBe(dto.EnrollmentDate.GetValueOrDefault());
    }
}