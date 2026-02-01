using System.Collections.Generic;
using System.Threading.Tasks;
using ContosoUniversity.Domain.Features.Students;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContosoUniversity.Pages;

public class AboutPage : PageModel
{
    private readonly IStudentService _studentService;

    public AboutPage(IStudentService studentService)
    {
        _studentService = studentService;
    }

    public IEnumerable<EnrollmentDateGroupDto> Data { get; private set; }

    public async Task OnGetAsync()
    {
        Data = await _studentService.GetEnrollmentDateGroupsAsync();
    }
}