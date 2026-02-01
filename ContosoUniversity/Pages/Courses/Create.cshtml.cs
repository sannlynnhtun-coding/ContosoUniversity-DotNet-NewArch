using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Domain.Features.Courses;
using ContosoUniversity.Domain.Features.Departments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ContosoUniversity.Pages.Courses;

public class Create : PageModel
{
    private readonly ICourseService _courseService;
    private readonly IDepartmentService _departmentService;

    public Create(ICourseService courseService, IDepartmentService departmentService)
    {
        _courseService = courseService;
        _departmentService = departmentService;
    }

    public SelectList Departments { get; set; }

    [BindProperty]
    public CourseEditDto Data { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        await PopulateDepartmentsDropDownList();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await PopulateDepartmentsDropDownList();
            return Page();
        }

        await _courseService.CreateCourseAsync(Data);

        return RedirectToPage("./Index");
    }

    private async Task PopulateDepartmentsDropDownList(object selectedDepartment = null)
    {
        var departments = await _departmentService.GetDepartmentNamesAsync();
        Departments = new SelectList(departments, "Id", "Name", selectedDepartment);
    }
}