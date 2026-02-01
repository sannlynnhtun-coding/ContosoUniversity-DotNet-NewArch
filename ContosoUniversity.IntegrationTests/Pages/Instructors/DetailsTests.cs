using System;
using System.Threading.Tasks;
using ContosoUniversity.Domain.Features.Instructors;
using Shouldly;
using Xunit;

namespace ContosoUniversity.IntegrationTests.Pages.Instructors;

[Collection(nameof(SliceFixture))]
public class DetailsTests : SliceTestBase
{
    public DetailsTests(SliceFixture fixture) : base(fixture) { }

    [Fact]
    public async Task Should_get_instructor_details()
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
}