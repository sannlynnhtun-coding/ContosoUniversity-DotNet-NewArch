using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContosoUniversity.Domain.Features.Departments;

public interface IDepartmentService
{
    Task<List<DepartmentNameDto>> GetDepartmentNamesAsync();
    Task<List<DepartmentListDto>> GetDepartmentsAsync();
    Task<DepartmentDetailDto> GetDepartmentAsync(int id);
    Task CreateDepartmentAsync(DepartmentEditDto departmentDto);
    Task UpdateDepartmentAsync(DepartmentEditDto departmentDto);
    Task DeleteDepartmentAsync(int id);
    Task<DepartmentEditDto> GetDepartmentForEditAsync(int id);
}
