using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Domain.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Domain.Features.Departments;

public class DepartmentService : IDepartmentService
{
    private readonly SchoolContext _context;

    public DepartmentService(SchoolContext context)
    {
        _context = context;
    }

    public async Task<List<DepartmentNameDto>> GetDepartmentNamesAsync()
    {
        return await _context.Departments
            .OrderBy(d => d.Name)
            .Select(d => new DepartmentNameDto
            {
                Id = d.Id,
                Name = d.Name
            })
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<DepartmentListDto>> GetDepartmentsAsync()
    {
        return await _context.Departments
            .Include(d => d.Administrator)
            .AsNoTracking()
            .Select(d => new DepartmentListDto
            {
                Id = d.Id,
                Name = d.Name,
                Budget = d.Budget,
                StartDate = d.StartDate,
                AdministratorName = d.Administrator != null ? d.Administrator.LastName + ", " + d.Administrator.FirstMidName : null
            })
            .ToListAsync();
    }

    public async Task<DepartmentDetailDto> GetDepartmentAsync(int id)
    {
        var d = await _context.Departments
            .Include(d => d.Administrator)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id);

        if (d == null) return null;

        return new DepartmentDetailDto
        {
            Id = d.Id,
            Name = d.Name,
            Budget = d.Budget,
            StartDate = d.StartDate,
            InstructorId = d.InstructorId,
            AdministratorName = d.Administrator != null ? d.Administrator.LastName + ", " + d.Administrator.FirstMidName : null,
            RowVersion = d.RowVersion
        };
    }

    public async Task CreateDepartmentAsync(DepartmentEditDto departmentDto)
    {
        var department = new Department
        {
            Name = departmentDto.Name,
            Budget = departmentDto.Budget,
            StartDate = departmentDto.StartDate,
            InstructorId = departmentDto.InstructorId
        };

        _context.Departments.Add(department);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateDepartmentAsync(DepartmentEditDto departmentDto)
    {
        // Concurrency handling for RowVersion is usually tricky with manual mapping.
        // We need to fetch, check RowVersion, update.
        // Or attach and set OriginalValue for RowVersion.
        
        var departmentToUpdate = await _context.Departments.FindAsync(departmentDto.Id);
        if (departmentToUpdate == null) return; // Or throw

        _context.Entry(departmentToUpdate).Property(p => p.RowVersion).OriginalValue = departmentDto.RowVersion;

        departmentToUpdate.Name = departmentDto.Name;
        departmentToUpdate.Budget = departmentDto.Budget;
        departmentToUpdate.StartDate = departmentDto.StartDate;
        departmentToUpdate.InstructorId = departmentDto.InstructorId;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            throw; // Page should handle this
        }
    }

    public async Task DeleteDepartmentAsync(int id)
    {
        var department = await _context.Departments.FindAsync(id);
        if (department != null)
        {
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
        }
    }
}
