using System.Threading.Tasks;
using ContosoUniversity.Domain.Features.Courses;
using ContosoUniversity.Domain.Features.Departments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ContosoUniversity.Pages.Courses;

public class Edit : PageModel
{
    private readonly ICourseService _courseService;
    private readonly IDepartmentService _departmentService;

    public Edit(ICourseService courseService, IDepartmentService departmentService)
    {
        _courseService = courseService;
        _departmentService = departmentService;
    }

    [BindProperty]
    public CourseEditDto Data { get; set; }

    public SelectList Departments { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var course = await _courseService.GetCourseAsync(id);
        if (course == null)
        {
            return NotFound();
        }

        Data = new CourseEditDto
        {
            Id = course.Id,
            Title = course.Title,
            Credits = course.Credits,
            DepartmentId = course.DepartmentId
        };

        await PopulateDepartmentsDropDownList(course.DepartmentId);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await PopulateDepartmentsDropDownList(Data.DepartmentId);
            return Page();
        }

        await _courseService.UpdateCourseAsync(Data);

        return RedirectToPage("./Index");
    }

    private async Task PopulateDepartmentsDropDownList(object selectedDepartment = null)
    {
        var departments = await _departmentService.GetDepartmentNamesAsync();
        Departments = new SelectList(departments, "Id", "Name", selectedDepartment);
    }
}