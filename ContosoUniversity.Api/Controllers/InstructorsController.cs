using Microsoft.AspNetCore.Mvc;
using ContosoUniversity.Domain.Features.Instructors;

namespace ContosoUniversity.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InstructorsController : ControllerBase
{
    private readonly IInstructorService _instructorService;

    public InstructorsController(IInstructorService instructorService)
    {
        _instructorService = instructorService;
    }

    [HttpGet]
    public async Task<ActionResult<List<InstructorListDto>>> GetInstructors()
    {
        return await _instructorService.GetInstructorsAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<InstructorDetailDto>> GetInstructor(int id)
    {
        var instructor = await _instructorService.GetInstructorAsync(id);
        if (instructor == null) return NotFound();
        return instructor;
    }

    [HttpPost]
    public async Task<IActionResult> CreateInstructor(InstructorEditDto instructorDto)
    {
        await _instructorService.CreateInstructorAsync(instructorDto);
        return CreatedAtAction(nameof(GetInstructor), new { id = instructorDto.Id }, instructorDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateInstructor(int id, InstructorEditDto instructorDto)
    {
        if (id != instructorDto.Id) return BadRequest();
        await _instructorService.UpdateInstructorAsync(instructorDto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteInstructor(int id)
    {
        await _instructorService.DeleteInstructorAsync(id);
        return NoContent();
    }
}
