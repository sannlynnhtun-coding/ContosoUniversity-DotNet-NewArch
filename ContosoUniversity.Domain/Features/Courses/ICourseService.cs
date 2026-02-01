using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContosoUniversity.Domain.Features.Courses;

public interface ICourseService
{
    Task<List<CourseListDto>> GetCoursesAsync();
    Task<CourseDetailDto> GetCourseAsync(int id);
    Task CreateCourseAsync(CourseEditDto courseDto);
    Task UpdateCourseAsync(CourseEditDto courseDto);
    Task DeleteCourseAsync(int id);
}
