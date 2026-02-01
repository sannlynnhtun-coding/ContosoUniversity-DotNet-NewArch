using System.Threading.Tasks;
using ContosoUniversity.Domain.Features.Students;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContosoUniversity.Pages.Students;

public class Details : PageModel
{
    private readonly IStudentService _studentService;

    public Details(IStudentService studentService)
    {
        _studentService = studentService;
    }

    public StudentDetailDto Data { get; private set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Data = await _studentService.GetStudentAsync(id);

        if (Data == null)
        {
            return NotFound();
        }
        return Page();
    }
}