using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Domain.Features.Courses;
using ContosoUniversity.Domain.Features.Instructors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContosoUniversity.Pages.Instructors;

public class CreateEdit : PageModel
{
    private readonly IInstructorService _instructorService;
    private readonly ICourseService _courseService;

    public CreateEdit(IInstructorService instructorService, ICourseService courseService)
    {
        _instructorService = instructorService;
        _courseService = courseService;
    }

    [BindProperty]
    public InstructorEditDto Data { get; set; }

    public List<AssignedCourseDto> AssignedCourses { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        var allCourses = await _courseService.GetCoursesAsync();
        
        if (id.HasValue)
        {
            var instructor = await _instructorService.GetInstructorAsync(id.Value);
            if (instructor == null) return NotFound();

            Data = new InstructorEditDto
            {
                Id = instructor.Id,
                LastName = instructor.LastName,
                FirstMidName = instructor.FirstMidName,
                HireDate = instructor.HireDate,
                OfficeAssignmentLocation = instructor.OfficeAssignmentLocation,
                SelectedCourses = instructor.CourseIds?.ToList() ?? new List<int>()
            };
        }
        else
        {
            Data = new InstructorEditDto();
        }

        PopulateAssignedCourseData(allCourses, Data.SelectedCourses);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var allCourses = await _courseService.GetCoursesAsync(); // Repopulate for redisplay if error

        if (!ModelState.IsValid)
        {
            PopulateAssignedCourseData(allCourses, Data.SelectedCourses);
            return Page();
        }

        if (Data.Id == 0)
        {
            await _instructorService.CreateInstructorAsync(Data);
        }
        else
        {
            await _instructorService.UpdateInstructorAsync(Data);
        }

        return RedirectToPage("./Index");
    }

    private void PopulateAssignedCourseData(List<CourseListDto> allCourses, List<int> instructorCourses)
    {
        var instructorCourseHS = new HashSet<int>(instructorCourses ?? new List<int>());
        AssignedCourses = new List<AssignedCourseDto>();
        foreach (var course in allCourses)
        {
            AssignedCourses.Add(new AssignedCourseDto
            {
                CourseId = course.Id,
                Title = course.Title,
                Assigned = instructorCourseHS.Contains(course.Id)
            });
        }
    }
}