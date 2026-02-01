using Microsoft.AspNetCore.Mvc;
using ContosoUniversity.Domain.Features.Departments;

namespace ContosoUniversity.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentsController : ControllerBase
{
    private readonly IDepartmentService _departmentService;

    public DepartmentsController(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    [HttpGet]
    public async Task<ActionResult<List<DepartmentListDto>>> GetDepartments()
    {
        return await _departmentService.GetDepartmentsAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DepartmentDetailDto>> GetDepartment(int id)
    {
        var department = await _departmentService.GetDepartmentAsync(id);
        if (department == null) return NotFound();
        return department;
    }

    [HttpPost]
    public async Task<IActionResult> CreateDepartment(DepartmentEditDto departmentDto)
    {
        await _departmentService.CreateDepartmentAsync(departmentDto);
        return CreatedAtAction(nameof(GetDepartment), new { id = departmentDto.Id }, departmentDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDepartment(int id, DepartmentEditDto departmentDto)
    {
        if (id != departmentDto.Id) return BadRequest();
        await _departmentService.UpdateDepartmentAsync(departmentDto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDepartment(int id)
    {
        await _departmentService.DeleteDepartmentAsync(id);
        return NoContent();
    }
}
