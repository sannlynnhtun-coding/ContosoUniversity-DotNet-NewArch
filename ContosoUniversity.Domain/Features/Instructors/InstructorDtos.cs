using System;
using System.Collections.Generic;

namespace ContosoUniversity.Domain.Features.Instructors;

public record InstructorNameDto
{
    public int Id { get; init; }
    public string FullName { get; init; }
}

public record InstructorListDto
{
    public int Id { get; init; }
    public string LastName { get; init; }
    public string FirstMidName { get; init; }
    public DateTime HireDate { get; init; }
    public string OfficeAssignmentLocation { get; init; }
    public IEnumerable<InstructorCourseDto> CourseAssignments { get; init; }
}

public record InstructorDetailDto
{
    public int Id { get; init; }
    public string LastName { get; init; }
    public string FirstMidName { get; init; }
    public DateTime HireDate { get; init; }
    public string OfficeAssignmentLocation { get; init; }
    public IEnumerable<int> CourseIds { get; init; }
}

public record InstructorCourseDto
{
    public int CourseId { get; init; }
    public string CourseTitle { get; init; }
    public string DepartmentName { get; init; }
}

public record InstructorEditDto
{
    public int Id { get; init; }
    public string LastName { get; init; }
    public string FirstMidName { get; init; }
    public DateTime HireDate { get; init; }
    public string OfficeAssignmentLocation { get; init; }
    public List<int> SelectedCourses { get; init; } = new();
}

public record AssignedCourseDto
{
    public int CourseId { get; init; }
    public string Title { get; init; }
    public bool Assigned { get; set; }
}
