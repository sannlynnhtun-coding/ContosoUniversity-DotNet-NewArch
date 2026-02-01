using System.Collections.Generic;
using System.Threading.Tasks;
using ContosoUniversity.Domain.Features.Courses;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContosoUniversity.Pages.Courses;

public class Index : PageModel
{
    private readonly ICourseService _courseService;

    public Index(ICourseService courseService)
    {
        _courseService = courseService;
    }

    public List<CourseListDto> Courses { get; private set; }

    public async Task OnGetAsync()
    {
        Courses = await _courseService.GetCoursesAsync();
    }
}