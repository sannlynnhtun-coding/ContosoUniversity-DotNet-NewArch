using System.Threading.Tasks;
using ContosoUniversity.Domain.Features.Instructors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContosoUniversity.Pages.Instructors;

public class Details : PageModel
{
    private readonly IInstructorService _instructorService;

    public Details(IInstructorService instructorService)
    {
        _instructorService = instructorService;
    }

    public InstructorDetailDto Data { get; private set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Data = await _instructorService.GetInstructorAsync(id);

        if (Data == null)
        {
            return NotFound();
        }
        return Page();
    }
}