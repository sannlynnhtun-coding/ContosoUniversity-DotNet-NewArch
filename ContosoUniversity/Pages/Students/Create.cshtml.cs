using System.Threading.Tasks;
using ContosoUniversity.Domain.Features.Students;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContosoUniversity.Pages.Students;

public class Create : PageModel
{
    private readonly IStudentService _studentService;

    public Create(IStudentService studentService)
    {
        _studentService = studentService;
    }

    [BindProperty]
    public StudentEditDto Data { get; set; }

    public void OnGet() => Data = new StudentEditDto();

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        await _studentService.CreateStudentAsync(Data);

        return RedirectToPage("./Index");
    }
}