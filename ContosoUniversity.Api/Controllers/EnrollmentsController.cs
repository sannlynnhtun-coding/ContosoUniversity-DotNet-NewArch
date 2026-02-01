using Microsoft.AspNetCore.Mvc;
using ContosoUniversity.Domain.Features.Enrollments;

namespace ContosoUniversity.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnrollmentsController : ControllerBase
{
    private readonly IEnrollmentService _enrollmentService;

    public EnrollmentsController(IEnrollmentService enrollmentService)
    {
        _enrollmentService = enrollmentService;
    }

    [HttpGet("course/{courseId}")]
    public async Task<ActionResult<List<EnrollmentListDto>>> GetEnrollmentsForCourse(int courseId)
    {
        return await _enrollmentService.GetEnrollmentsForCourseAsync(courseId);
    }
}
