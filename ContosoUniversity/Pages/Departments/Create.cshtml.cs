using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Domain.Features.Departments;
using ContosoUniversity.Domain.Features.Instructors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ContosoUniversity.Pages.Departments;

public class Create : PageModel
{
    private readonly IDepartmentService _departmentService;
    private readonly IInstructorService _instructorService;

    public Create(IDepartmentService departmentService, IInstructorService instructorService)
    {
        _departmentService = departmentService;
        _instructorService = instructorService;
    }

    public SelectList Instructors { get; set; }

    [BindProperty]
    public DepartmentEditDto Data { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        await PopulateInstructorsDropDownList();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await PopulateInstructorsDropDownList();
            return Page();
        }

        await _departmentService.CreateDepartmentAsync(Data);

        return RedirectToPage("./Index");
    }

    private async Task PopulateInstructorsDropDownList(object selectedInstructor = null)
    {
        var instructors = await _instructorService.GetInstructorNamesAsync();
        Instructors = new SelectList(instructors, "Id", "FullName", selectedInstructor);
    }
}