using ContosoUniversity.Domain.Features.Courses;

namespace ContosoUniversity.Domain.Features.Instructors;

public class CourseAssignment
{
    public int InstructorId { get; set; }
    public int CourseId { get; set; }
    public Instructor Instructor { get; set; }
    public Course Course { get; set; }
}
