using System.Threading.Tasks;
using ContosoUniversity.Domain.Features.Departments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContosoUniversity.Pages.Departments;

public class Details : PageModel
{
    private readonly IDepartmentService _departmentService;

    public Details(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    public DepartmentDetailDto Data { get; private set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Data = await _departmentService.GetDepartmentAsync(id);

        if (Data == null)
        {
            return NotFound();
        }
        return Page();
    }
}