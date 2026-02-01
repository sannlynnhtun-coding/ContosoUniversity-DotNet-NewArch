using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContosoUniversity.Domain.Features.Instructors;

public interface IInstructorService
{
    Task<List<InstructorNameDto>> GetInstructorNamesAsync();
    Task<List<InstructorListDto>> GetInstructorsAsync();
    Task<InstructorDetailDto> GetInstructorAsync(int id);
    Task CreateInstructorAsync(InstructorEditDto instructorDto);
    Task UpdateInstructorAsync(InstructorEditDto instructorDto);
    Task DeleteInstructorAsync(int id);
}
