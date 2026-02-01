using Microsoft.AspNetCore.Mvc;
using ContosoUniversity.Domain.Features.Students;
using ContosoUniversity.Domain.Shared;

namespace ContosoUniversity.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentsController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedList<StudentListDto>>> GetStudents(string? sortOrder = null, string? searchString = null, int? pageIndex = 1, int pageSize = 10)
    {
        return await _studentService.GetStudentsAsync(sortOrder, searchString, pageIndex, pageSize);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<StudentDetailDto>> GetStudent(int id)
    {
        var student = await _studentService.GetStudentAsync(id);
        if (student == null) return NotFound();
        return student;
    }

    [HttpPost]
    public async Task<IActionResult> CreateStudent(StudentEditDto student)
    {
        await _studentService.CreateStudentAsync(student);
        return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, student);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStudent(int id, StudentEditDto student)
    {
        if (id != student.Id) return BadRequest();
        await _studentService.UpdateStudentAsync(student);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        await _studentService.DeleteStudentAsync(id);
        return NoContent();
    }
}
