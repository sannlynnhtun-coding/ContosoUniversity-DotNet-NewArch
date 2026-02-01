using System.Threading.Tasks;
using ContosoUniversity.Domain.Features.Students;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContosoUniversity.Pages.Students;

public class Edit : PageModel
{
    private readonly IStudentService _studentService;

    public Edit(IStudentService studentService)
    {
        _studentService = studentService;
    }

    [BindProperty]
    public StudentEditDto Data { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Data = await _studentService.GetStudentForEditAsync(id);

        if (Data == null)
        {
            return NotFound();
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        await _studentService.UpdateStudentAsync(Data);

        return RedirectToPage("./Index");
    }
}