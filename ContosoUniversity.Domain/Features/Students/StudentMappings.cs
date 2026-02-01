using System;
using System.Linq;
using System.Linq.Expressions;

namespace ContosoUniversity.Domain.Features.Students;

public static class StudentMappings
{
    public static Expression<Func<Student, StudentListDto>> ToListDtoExpression = student => new StudentListDto
    {
        Id = student.Id,
        FirstMidName = student.FirstMidName,
        LastName = student.LastName,
        EnrollmentDate = student.EnrollmentDate,
        EnrollmentsCount = student.Enrollments.Count
    };

    public static StudentListDto ToListDto(Student student)
    {
        return new StudentListDto
        {
            Id = student.Id,
            FirstMidName = student.FirstMidName,
            LastName = student.LastName,
            EnrollmentDate = student.EnrollmentDate,
            EnrollmentsCount = student.Enrollments.Count
        };
    }
}
