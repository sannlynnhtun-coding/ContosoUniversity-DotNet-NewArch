using System;
using System.Threading.Tasks;
using ContosoUniversity.Domain.Features.Students;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;

namespace ContosoUniversity.IntegrationTests.Pages.Students;

[Collection(nameof(SliceFixture))]
public class DeleteTests : SliceTestBase
{
    public DeleteTests(SliceFixture fixture) : base(fixture) { }

    [Fact]
    public async Task Should_get_delete_details()
    {
        var student = new Student
        {
            FirstMidName = "Joe",
            LastName = "Schmoe",
            EnrollmentDate = DateTime.Today
        };
        await Fixture.InsertAsync(student);

        var result = await Fixture.ExecuteServiceAsync<IStudentService, StudentDetailDto>(s => 
            s.GetStudentAsync(student.Id));

        result.FirstMidName.ShouldBe(student.FirstMidName);
        result.LastName.ShouldBe(student.LastName);
        result.EnrollmentDate.ShouldBe(student.EnrollmentDate);
    }

    [Fact]
    public async Task Should_delete_student()
    {
        var student = new Student
        {
            FirstMidName = "Joe",
            LastName = "Schmoe",
            EnrollmentDate = DateTime.Today
        };
        await Fixture.InsertAsync(student);

        await Fixture.ExecuteServiceAsync<IStudentService>(s => s.DeleteStudentAsync(student.Id));

        var dbStudent = await Fixture.FindAsync<Student>(student.Id);

        dbStudent.ShouldBeNull();
    }
}