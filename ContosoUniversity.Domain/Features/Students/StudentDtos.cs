using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ContosoUniversity.Domain.Features.Students;

public record StudentListDto
{
    public int Id { get; init; }
    public string FirstMidName { get; init; }
    public string LastName { get; init; }
    public DateTime EnrollmentDate { get; init; }
    public int EnrollmentsCount { get; init; }
}

public record StudentDetailDto
{
    public int Id { get; init; }
    public string LastName { get; init; }
    public string FirstMidName { get; init; }
    public DateTime EnrollmentDate { get; init; }
    // Enrollments info
    public IEnumerable<StudentEnrollmentDto> Enrollments { get; init; }
}

public record StudentEditDto
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string LastName { get; set; }

    [Required]
    [StringLength(50)]
    [Display(Name = "First Mid Name")]
    public string FirstMidName { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime? EnrollmentDate { get; set; }
}

public record StudentEnrollmentDto
{
    public string CourseTitle { get; init; }
    public string Grade { get; init; }
}

public record EnrollmentDateGroupDto
{
    [DataType(DataType.Date)]
    public DateTime? EnrollmentDate { get; init; }

    public int StudentCount { get; init; }
}
