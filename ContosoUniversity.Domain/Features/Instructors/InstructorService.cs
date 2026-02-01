using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Domain.Features.Courses;
using ContosoUniversity.Domain.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Domain.Features.Instructors;

public class InstructorService : IInstructorService
{
    private readonly SchoolContext _context;

    public InstructorService(SchoolContext context)
    {
        _context = context;
    }

    public async Task<List<InstructorNameDto>> GetInstructorNamesAsync()
    {
        return await _context.Instructors
            .OrderBy(i => i.LastName)
            .Select(i => new InstructorNameDto
            {
                Id = i.Id,
                FullName = i.LastName + ", " + i.FirstMidName
            })
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<InstructorListDto>> GetInstructorsAsync()
    {
        var instructors = await _context.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.CourseAssignments)
                    .ThenInclude(c => c.Course)
                        .ThenInclude(c => c.Department)
                .OrderBy(i => i.LastName)
                .Select(i => new InstructorListDto
                {
                    Id = i.Id,
                    LastName = i.LastName,
                    FirstMidName = i.FirstMidName,
                    HireDate = i.HireDate,
                    OfficeAssignmentLocation = i.OfficeAssignment.Location,
                    CourseAssignments = i.CourseAssignments.Select(ca => new InstructorCourseDto
                    {
                        CourseId = ca.Course.Id,
                        CourseTitle = ca.Course.Title,
                        DepartmentName = ca.Course.Department.Name
                    })
                })
                .AsNoTracking()
                .ToListAsync();
        return instructors;
    }

    public async Task<InstructorDetailDto> GetInstructorAsync(int id)
    {
        return await _context.Instructors
            .Include(i => i.OfficeAssignment)
            .Include(i => i.CourseAssignments)
            .AsNoTracking()
            .Where(i => i.Id == id)
            .Select(i => new InstructorDetailDto
            {
                Id = i.Id,
                LastName = i.LastName,
                FirstMidName = i.FirstMidName,
                HireDate = i.HireDate,
                OfficeAssignmentLocation = i.OfficeAssignment.Location,
                CourseIds = i.CourseAssignments.Select(c => c.CourseId)
            })
            .FirstOrDefaultAsync();
    }

    public async Task CreateInstructorAsync(InstructorEditDto instructorDto)
    {
        var instructor = new Instructor
        {
            FirstMidName = instructorDto.FirstMidName,
            LastName = instructorDto.LastName,
            HireDate = instructorDto.HireDate
        };

        if (!string.IsNullOrWhiteSpace(instructorDto.OfficeAssignmentLocation))
        {
            instructor.OfficeAssignment = new OfficeAssignment { Location = instructorDto.OfficeAssignmentLocation };
        }

        if (instructorDto.SelectedCourses != null)
        {
            instructor.CourseAssignments = new List<CourseAssignment>();
            foreach (var courseId in instructorDto.SelectedCourses)
            {
                instructor.CourseAssignments.Add(new CourseAssignment { CourseId = courseId });
            }
        }

        _context.Instructors.Add(instructor);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateInstructorAsync(InstructorEditDto instructorDto)
    {
        var instructor = await _context.Instructors
            .Include(i => i.OfficeAssignment)
            .Include(i => i.CourseAssignments)
            .FirstOrDefaultAsync(i => i.Id == instructorDto.Id);

        if (instructor == null) return;

        instructor.LastName = instructorDto.LastName;
        instructor.FirstMidName = instructorDto.FirstMidName;
        instructor.HireDate = instructorDto.HireDate;

        if (string.IsNullOrWhiteSpace(instructorDto.OfficeAssignmentLocation))
        {
            instructor.OfficeAssignment = null;
        }
        else
        {
            if (instructor.OfficeAssignment == null)
            {
                instructor.OfficeAssignment = new OfficeAssignment { Location = instructorDto.OfficeAssignmentLocation };
            }
            else
            {
                instructor.OfficeAssignment.Location = instructorDto.OfficeAssignmentLocation;
            }
        }

        UpdateInstructorCourses(instructorDto.SelectedCourses, instructor);

        await _context.SaveChangesAsync();
    }

    private void UpdateInstructorCourses(List<int> selectedCourses, Instructor instructor)
    {
        if (selectedCourses == null)
        {
            instructor.CourseAssignments = new List<CourseAssignment>();
            return;
        }

        var selectedCoursesHS = new HashSet<int>(selectedCourses);
        var instructorCourses = new HashSet<int>(instructor.CourseAssignments.Select(c => c.CourseId));

        foreach (var courseId in selectedCoursesHS)
        {
            if (!instructorCourses.Contains(courseId))
            {
                instructor.CourseAssignments.Add(new CourseAssignment { InstructorId = instructor.Id, CourseId = courseId });
            }
        }

        foreach (var courseId in instructorCourses)
        {
            if (!selectedCoursesHS.Contains(courseId))
            {
                var courseToRemove = instructor.CourseAssignments.SingleOrDefault(i => i.CourseId == courseId);
                if (courseToRemove != null)
                {
                    _context.Remove(courseToRemove);
                }
            }
        }
    }

    public async Task DeleteInstructorAsync(int id)
    {
        var instructor = await _context.Instructors
            .Include(i => i.OfficeAssignment)
            .SingleOrDefaultAsync(i => i.Id == id);

        if (instructor == null) return;

        var departments = await _context.Departments
            .Where(d => d.InstructorId == id)
            .ToListAsync();

        foreach (var d in departments)
        {
            d.InstructorId = null;
        }

        _context.Instructors.Remove(instructor);
        await _context.SaveChangesAsync();
    }
}
