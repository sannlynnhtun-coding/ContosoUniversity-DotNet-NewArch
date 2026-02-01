using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ContosoUniversity.Domain.Shared;

namespace ContosoUniversity.Domain.Features.Instructors;

public class Instructor : IEntity
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string LastName { get; set; }

    [Required]
    [Column("FirstName")]
    [StringLength(50)]
    public string FirstMidName { get; set; }

    [DataType(DataType.Date)]
    public DateTime HireDate { get; set; }

    [NotMapped]
    public string FullName => LastName + ", " + FirstMidName;

    public ICollection<CourseAssignment> CourseAssignments { get; set; } = new List<CourseAssignment>();
    public OfficeAssignment OfficeAssignment { get; set; }
}
