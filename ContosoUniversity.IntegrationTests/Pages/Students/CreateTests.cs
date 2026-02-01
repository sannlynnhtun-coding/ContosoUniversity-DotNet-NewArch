using System;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Domain.Features.Students;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;

namespace ContosoUniversity.IntegrationTests.Pages.Students;

[Collection(nameof(SliceFixture))]
public class CreateTests : SliceTestBase
{
    public CreateTests(SliceFixture fixture) : base(fixture) { }

    [Fact]
    public async Task Should_create_student()
    {
        var dto = new StudentEditDto
        {
            FirstMidName = "Joe",
            LastName = "Schmoe",
            EnrollmentDate = DateTime.Today
        };

        await Fixture.ExecuteServiceAsync<IStudentService>(s => s.CreateStudentAsync(dto));

        var student = await Fixture.ExecuteDbContextAsync(db => db.Students
            .Where(s => s.LastName == "Schmoe" && s.FirstMidName == "Joe")
            .FirstOrDefaultAsync());

        student.ShouldNotBeNull();
        student.FirstMidName.ShouldBe(dto.FirstMidName);
        student.LastName.ShouldBe(dto.LastName);
        student.EnrollmentDate.ShouldBe(dto.EnrollmentDate.GetValueOrDefault());
    }
}