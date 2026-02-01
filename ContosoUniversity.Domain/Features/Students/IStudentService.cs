using System.Threading.Tasks;
using ContosoUniversity.Domain.Shared;

namespace ContosoUniversity.Domain.Features.Students;

public interface IStudentService
{
    Task<PaginatedList<StudentListDto>> GetStudentsAsync(string sortOrder, string searchString, int? pageIndex, int pageSize);
    Task CreateStudentAsync(StudentEditDto student);
    Task UpdateStudentAsync(StudentEditDto student);
    Task DeleteStudentAsync(int id);
    Task<StudentDetailDto> GetStudentAsync(int id); 
    Task<System.Collections.Generic.List<EnrollmentDateGroupDto>> GetEnrollmentDateGroupsAsync();
    Task<StudentEditDto> GetStudentForEditAsync(int id);
}
