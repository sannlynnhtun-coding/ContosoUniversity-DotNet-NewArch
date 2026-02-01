using System.Collections.Generic;
using System.Threading.Tasks;
using ContosoUniversity.Domain.Features.Departments;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContosoUniversity.Pages.Departments;

public class Index : PageModel
{
    private readonly IDepartmentService _departmentService;

    public Index(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    public List<DepartmentListDto> Data { get; private set; }

    public async Task OnGetAsync()
    {
        Data = await _departmentService.GetDepartmentsAsync();
    }
}