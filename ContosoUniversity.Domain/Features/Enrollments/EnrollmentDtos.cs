using ContosoUniversity.Domain.Features.Courses;

namespace ContosoUniversity.Domain.Features.Enrollments;

public record EnrollmentListDto
{
    public string StudentFullName { get; init; }
    public Grade? Grade { get; init; }
}
