using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Domain.Features.Students;
using ContosoUniversity.Domain.Shared;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContosoUniversity.Pages.Students;

public class Index : PageModel
{
    private readonly IStudentService _studentService;

    public Index(IStudentService studentService)
    {
        _studentService = studentService;
    }

    public Result Data { get; private set; }

    public async Task OnGetAsync(string sortOrder,
        string currentFilter, string searchString, int? pageIndex)
    {
        if (searchString != null)
        {
            pageIndex = 1;
        }
        else
        {
            searchString = currentFilter;
        }

        var result = await _studentService.GetStudentsAsync(sortOrder, searchString, pageIndex, 3);
        
        var viewModels = result.Select(s => new Model
        {
            Id = s.Id,
            FirstMidName = s.FirstMidName,
            LastName = s.LastName,
            EnrollmentDate = s.EnrollmentDate,
            EnrollmentsCount = s.EnrollmentsCount
        }).ToList();

        Data = new Result
        {
            CurrentSort = sortOrder,
            NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "",
            DateSortParm = sortOrder == "Date" ? "date_desc" : "Date",
            CurrentFilter = searchString,
            SearchString = searchString,
            Results = new PaginatedList<Model>(viewModels, result.TotalCount, result.PageIndex, 3) 
        };
    }

    public record Result
    {
        public string CurrentSort { get; init; }
        public string NameSortParm { get; init; }
        public string DateSortParm { get; init; }
        public string CurrentFilter { get; init; }
        public string SearchString { get; init; }

        public PaginatedList<Model> Results { get; init; }
    }

    public record Model
    {
        public int Id { get; init; }
        [Display(Name = "First Name")]
        public string FirstMidName { get; init; }
        public string LastName { get; init; }
        public DateTime EnrollmentDate { get; init; }
        public int EnrollmentsCount { get; init; }
    }
}