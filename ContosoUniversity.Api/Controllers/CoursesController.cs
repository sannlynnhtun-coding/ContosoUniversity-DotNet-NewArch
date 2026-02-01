using Microsoft.AspNetCore.Mvc;
using ContosoUniversity.Domain.Features.Courses;

namespace ContosoUniversity.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _courseService;

    public CoursesController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpGet]
    public async Task<ActionResult<List<CourseListDto>>> GetCourses()
    {
        return await _courseService.GetCoursesAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CourseDetailDto>> GetCourse(int id)
    {
        var course = await _courseService.GetCourseAsync(id);
        if (course == null) return NotFound();
        return course;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCourse(CourseEditDto courseDto)
    {
        await _courseService.CreateCourseAsync(courseDto);
        return CreatedAtAction(nameof(GetCourse), new { id = courseDto.Id }, courseDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCourse(int id, CourseEditDto courseDto)
    {
        if (id != courseDto.Id) return BadRequest();
        await _courseService.UpdateCourseAsync(courseDto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCourse(int id)
    {
        await _courseService.DeleteCourseAsync(id);
        return NoContent();
    }
}
