using System.Threading.Tasks;
using ContosoUniversity.Domain.Features.Courses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContosoUniversity.Pages.Courses;

public class Delete : PageModel
{
    private readonly ICourseService _courseService;

    public Delete(ICourseService courseService)
    {
        _courseService = courseService;
    }

    [BindProperty]
    public CourseDetailDto Data { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Data = await _courseService.GetCourseAsync(id);

        if (Data == null)
        {
            return NotFound();
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        await _courseService.DeleteCourseAsync(id);

        return RedirectToPage("./Index");
    }
}