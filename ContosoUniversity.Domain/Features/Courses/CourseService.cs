using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Domain.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Domain.Features.Courses;

public class CourseService : ICourseService
{
    private readonly SchoolContext _context;

    public CourseService(SchoolContext context)
    {
        _context = context;
    }

    public async Task<List<CourseListDto>> GetCoursesAsync()
    {
        return await _context.Courses
            .Include(c => c.Department)
            .AsNoTracking()
            .Select(c => new CourseListDto
            {
                Id = c.Id,
                Title = c.Title,
                Credits = c.Credits,
                DepartmentName = c.Department.Name
            })
            .ToListAsync();
    }

    public async Task<CourseDetailDto> GetCourseAsync(int id)
    {
        var course = await _context.Courses
            .Include(c => c.Department)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

        if (course == null) return null;

        return new CourseDetailDto
        {
            Id = course.Id,
            Title = course.Title,
            Credits = course.Credits,
            DepartmentId = course.DepartmentId,
            DepartmentName = course.Department.Name
        };
    }

    public async Task CreateCourseAsync(CourseEditDto courseDto)
    {
        var course = new Course
        {
            Id = courseDto.Id, // Manual ID
            Title = courseDto.Title,
            Credits = courseDto.Credits,
            DepartmentId = courseDto.DepartmentId
        };

        _context.Courses.Add(course);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCourseAsync(CourseEditDto courseDto)
    {
        var course = await _context.Courses.FindAsync(courseDto.Id);
        if (course == null) return;

        course.Title = courseDto.Title;
        course.Credits = courseDto.Credits;
        course.DepartmentId = courseDto.DepartmentId;

        _context.Courses.Update(course);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCourseAsync(int id)
    {
        var course = await _context.Courses.FindAsync(id);
        if (course != null)
        {
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
        }
    }
}
