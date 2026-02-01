using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Domain.Features.Enrollments;
using ContosoUniversity.Domain.Features.Instructors;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContosoUniversity.Pages.Instructors;

public class Index : PageModel
{
    private readonly IInstructorService _instructorService;
    private readonly IEnrollmentService _enrollmentService;

    public Index(IInstructorService instructorService, IEnrollmentService enrollmentService)
    {
        _instructorService = instructorService;
        _enrollmentService = enrollmentService;
    }

    public List<InstructorListDto> Instructors { get; private set; }
    public List<InstructorCourseDto> Courses { get; private set; }
    public List<EnrollmentListDto> Enrollments { get; private set; }

    public int? InstructorId { get; set; }
    public int? CourseId { get; set; }

    public async Task OnGetAsync(int? id, int? courseId)
    {
        InstructorId = id;
        CourseId = courseId;

        Instructors = await _instructorService.GetInstructorsAsync();

        if (id != null)
        {
            var selectedInstructor = Instructors.FirstOrDefault(i => i.Id == id.Value);
            if (selectedInstructor != null && selectedInstructor.CourseAssignments != null)
            {
                Courses = selectedInstructor.CourseAssignments.ToList();
            }
        }

        if (courseId != null)
        {
            Enrollments = await _enrollmentService.GetEnrollmentsForCourseAsync(courseId.Value);
        }
    }
}