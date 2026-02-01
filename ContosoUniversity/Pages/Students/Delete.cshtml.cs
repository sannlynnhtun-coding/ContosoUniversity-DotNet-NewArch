using System.Threading.Tasks;
using ContosoUniversity.Domain.Features.Students;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContosoUniversity.Pages.Students;

public class Delete : PageModel
{
    private readonly IStudentService _studentService;

    public Delete(IStudentService studentService)
    {
        _studentService = studentService;
    }

    [BindProperty]
    public StudentDetailDto Data { get; set; }

    public string ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(int id, bool? saveChangesError = false)
    {
        if (saveChangesError.GetValueOrDefault())
        {
            ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
        }

        Data = await _studentService.GetStudentAsync(id);

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
            await _studentService.DeleteStudentAsync(id);
        }
        catch
        {
             return RedirectToPage("./Delete", new { id, saveChangesError = true });
        }

        return RedirectToPage("./Index");
    }
}