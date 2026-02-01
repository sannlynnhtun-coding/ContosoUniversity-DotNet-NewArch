using System.Threading.Tasks;
using ContosoUniversity.Domain.Features.Departments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Pages.Departments;

public class Delete : PageModel
{
    private readonly IDepartmentService _departmentService;

    public Delete(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    [BindProperty]
    public DepartmentDetailDto Data { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Data = await _departmentService.GetDepartmentAsync(id);

        if (Data == null)
        {
            return NotFound();
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        try
        {
            await _departmentService.DeleteDepartmentAsync(id);
        }
        catch (DbUpdateConcurrencyException)
        {
             // Log or handle
             return RedirectToPage("./Index");
        }
        
        return RedirectToPage("./Index");
    }
}