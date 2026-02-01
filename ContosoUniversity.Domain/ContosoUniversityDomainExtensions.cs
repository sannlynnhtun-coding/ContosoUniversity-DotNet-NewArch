using Microsoft.Extensions.DependencyInjection;
using ContosoUniversity.Domain.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Domain.Features.Students;
using ContosoUniversity.Domain.Features.Courses;
using ContosoUniversity.Domain.Features.Departments;
using ContosoUniversity.Domain.Features.Instructors;
using ContosoUniversity.Domain.Features.Enrollments;

namespace ContosoUniversity.Domain;

public static class ContosoUniversityDomainExtensions
{
    public static IServiceCollection AddContosoUniversityDomain(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<SchoolContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<ICourseService, CourseService>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<IInstructorService, InstructorService>();
        services.AddScoped<IEnrollmentService, EnrollmentService>();
        // Add other feature services here
        
        return services;
    }
}
