using System.Threading.Tasks;
using ContosoUniversity.Domain.Features.Departments;
using ContosoUniversity.Domain.Features.Instructors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Pages.Departments;

public class Edit : PageModel
{
    private readonly IDepartmentService _departmentService;
    private readonly IInstructorService _instructorService;

    public Edit(IDepartmentService departmentService, IInstructorService instructorService)
    {
        _departmentService = departmentService;
        _instructorService = instructorService;
    }

    [BindProperty]
    public DepartmentEditDto Data { get; set; }

    public SelectList Instructors { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var department = await _departmentService.GetDepartmentAsync(id);
        if (department == null)
        {
            return NotFound();
        }

        Data = new DepartmentEditDto
        {
            Id = department.Id,
            Name = department.Name,
            Budget = department.Budget,
            StartDate = department.StartDate,
            InstructorId = department.InstructorId,
            RowVersion = department.RowVersion
        };

        await PopulateInstructorsDropDownList(department.InstructorId);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await PopulateInstructorsDropDownList(Data.InstructorId);
            return Page();
        }

        try
        {
            await _departmentService.UpdateDepartmentAsync(Data);
        }
        catch (DbUpdateConcurrencyException)
        {
            // Simplified handling for now. 
            // In a full implementation, we'd reload the entity, show "Current Value" vs "Your Value".
            ModelState.AddModelError(string.Empty, "The record you attempted to edit "
              + "was modified by another user after you got the original value. The "
              + "edit operation was canceled.");
            await PopulateInstructorsDropDownList(Data.InstructorId);
            return Page();
        }

        return RedirectToPage("./Index");
    }

    private async Task PopulateInstructorsDropDownList(object selectedInstructor = null)
    {
        var instructors = await _instructorService.GetInstructorNamesAsync();
        Instructors = new SelectList(instructors, "Id", "FullName", selectedInstructor);
    }
}