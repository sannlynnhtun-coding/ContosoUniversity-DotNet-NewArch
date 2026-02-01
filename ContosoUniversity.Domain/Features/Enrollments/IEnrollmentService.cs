using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContosoUniversity.Domain.Features.Enrollments;

public interface IEnrollmentService
{
    Task<List<EnrollmentListDto>> GetEnrollmentsForCourseAsync(int courseId);
}
