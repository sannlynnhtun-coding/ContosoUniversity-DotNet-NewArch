using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ContosoUniversity.Domain.Shared;
using ContosoUniversity.Domain.Features.Instructors;
using ContosoUniversity.Domain.Features.Courses;

namespace ContosoUniversity.Domain.Features.Departments;

public class Department : IEntity
{
    [Column("DepartmentID")]
    public int Id { get; set; }

    [StringLength(50, MinimumLength = 3)]
    public string Name { get; set; }

    [DataType(DataType.Currency)]
    [Column(TypeName = "money")]
    public decimal Budget { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [Display(Name = "Start Date")]
    public DateTime StartDate { get; set; }

    public int? InstructorId { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; }

    public Instructor Administrator { get; set; }
    public ICollection<Course> Courses { get; set; }
}
