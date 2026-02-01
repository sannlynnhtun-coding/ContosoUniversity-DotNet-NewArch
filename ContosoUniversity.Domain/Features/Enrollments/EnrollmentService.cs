using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Domain.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Domain.Features.Enrollments;

public class EnrollmentService : IEnrollmentService
{
    private readonly SchoolContext _context;

    public EnrollmentService(SchoolContext context)
    {
        _context = context;
    }

    public async Task<List<EnrollmentListDto>> GetEnrollmentsForCourseAsync(int courseId)
    {
        return await _context.Enrollments
            .Include(e => e.Student)
            .Where(e => e.CourseId == courseId)
            .Select(e => new EnrollmentListDto
            {
                StudentFullName = e.Student.FullName,
                Grade = e.Grade
            })
            .AsNoTracking()
            .ToListAsync();
    }
}
