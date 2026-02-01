using System.Threading.Tasks;
using ContosoUniversity.Domain.Features.Courses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContosoUniversity.Pages.Courses;

public class Details : PageModel
{
    private readonly ICourseService _courseService;

    public Details(ICourseService courseService)
    {
        _courseService = courseService;
    }

    public CourseDetailDto Data { get; private set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Data = await _courseService.GetCourseAsync(id);

        if (Data == null)
        {
            return NotFound();
        }
        return Page();
    }
}