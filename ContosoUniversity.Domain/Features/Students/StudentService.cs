using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Domain.Infrastructure;
using ContosoUniversity.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Domain.Features.Students;

public class StudentService : IStudentService
{
    private readonly SchoolContext _context;

    public StudentService(SchoolContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<StudentListDto>> GetStudentsAsync(string sortOrder, string searchString, int? pageIndex, int pageSize)
    {
        IQueryable<Student> students = _context.Students.AsNoTracking();

        if (!string.IsNullOrEmpty(searchString))
        {
            students = students.Where(s => s.LastName.Contains(searchString)
                                           || s.FirstMidName.Contains(searchString));
        }

        students = sortOrder switch
        {
            "name_desc" => students.OrderByDescending(s => s.LastName),
            "Date" => students.OrderBy(s => s.EnrollmentDate),
            "date_desc" => students.OrderByDescending(s => s.EnrollmentDate),
            _ => students.OrderBy(s => s.LastName)
        };

        var source = students.Select(StudentMappings.ToListDtoExpression);

        return await PaginatedList<StudentListDto>.CreateAsync(source, pageIndex ?? 1, pageSize);
    }
    
    public async Task CreateStudentAsync(StudentEditDto studentDto)
    {
        var student = new Student
        {
            FirstMidName = studentDto.FirstMidName,
            LastName = studentDto.LastName,
            EnrollmentDate = studentDto.EnrollmentDate.Value
        };
        _context.Students.Add(student);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateStudentAsync(StudentEditDto studentDto)
    {
        var student = await _context.Students.FindAsync(studentDto.Id);
        if (student == null) return;

        student.LastName = studentDto.LastName;
        student.FirstMidName = studentDto.FirstMidName;
        student.EnrollmentDate = studentDto.EnrollmentDate.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteStudentAsync(int id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student != null)
        {
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<StudentDetailDto> GetStudentAsync(int id)
    {
        return await _context.Students
            .Include(s => s.Enrollments)
            .ThenInclude(e => e.Course)
            .AsNoTracking()
            .Where(m => m.Id == id)
            .Select(s => new StudentDetailDto
            {
                Id = s.Id,
                LastName = s.LastName,
                FirstMidName = s.FirstMidName,
                EnrollmentDate = s.EnrollmentDate,
                Enrollments = s.Enrollments.Select(e => new StudentEnrollmentDto
                {
                    CourseTitle = e.Course.Title,
                    Grade = e.Grade.ToString()
                })
            })
            .FirstOrDefaultAsync();
    }

    public async Task<StudentEditDto> GetStudentForEditAsync(int id)
    {
        return await _context.Students
            .AsNoTracking()
            .Where(m => m.Id == id)
            .Select(s => new StudentEditDto
            {
                Id = s.Id,
                LastName = s.LastName,
                FirstMidName = s.FirstMidName,
                EnrollmentDate = s.EnrollmentDate
            })
            .FirstOrDefaultAsync();
    }

    public async Task<List<EnrollmentDateGroupDto>> GetEnrollmentDateGroupsAsync()
    {
        var data = from student in _context.Students
                   group student by student.EnrollmentDate into dateGroup
                   select new EnrollmentDateGroupDto
                   {
                       EnrollmentDate = dateGroup.Key,
                       StudentCount = dateGroup.Count()
                   };

        return await data.AsNoTracking().ToListAsync();
    }
}
