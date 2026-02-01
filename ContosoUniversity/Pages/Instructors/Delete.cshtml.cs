using System.Threading.Tasks;
using ContosoUniversity.Domain.Features.Instructors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContosoUniversity.Pages.Instructors;

public class Delete : PageModel
{
    private readonly IInstructorService _instructorService;

    public Delete(IInstructorService instructorService)
    {
        _instructorService = instructorService;
    }

    [BindProperty]
    public InstructorDetailDto Data { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Data = await _instructorService.GetInstructorAsync(id);

        if (Data == null)
        {
            return NotFound();
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        await _instructorService.DeleteInstructorAsync(id);

        return RedirectToPage("./Index");
    }
}