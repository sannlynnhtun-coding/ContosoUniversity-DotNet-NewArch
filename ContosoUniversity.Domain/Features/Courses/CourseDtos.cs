namespace ContosoUniversity.Domain.Features.Courses;

public record CourseListDto
{
    public int Id { get; init; }
    public string Title { get; init; }
    public int Credits { get; init; }
    public string DepartmentName { get; init; }
}

public record CourseDetailDto
{
    public int Id { get; init; }
    public string Title { get; init; }
    public int Credits { get; init; }
    public int DepartmentId { get; init; }
    public string DepartmentName { get; init; }
}

// For Create/Update
public record CourseEditDto
{
    public int Id { get; init; } // Manual ID for create? Yes "Number" in Create page
    public string Title { get; init; }
    public int Credits { get; init; }
    public int DepartmentId { get; init; }
}
