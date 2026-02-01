using System;

namespace ContosoUniversity.Domain.Features.Departments;

public record DepartmentNameDto
{
    public int Id { get; init; }
    public string Name { get; init; }
}

public record DepartmentListDto
{
    public int Id { get; init; }
    public string Name { get; init; }
    public decimal Budget { get; init; }
    public DateTime StartDate { get; init; }
    public string AdministratorName { get; init; }
}

public record DepartmentDetailDto
{
    public int Id { get; init; }
    public string Name { get; init; }
    public decimal Budget { get; init; }
    public DateTime StartDate { get; init; }
    public int? InstructorId { get; init; } // Administrator
    public string AdministratorName { get; init; }
    public byte[] RowVersion { get; init; } // For concurrency
}

public record DepartmentEditDto
{
    public int Id { get; init; }
    public string Name { get; init; }
    public decimal Budget { get; init; }
    public DateTime StartDate { get; init; }
    public int? InstructorId { get; init; }
    public byte[] RowVersion { get; init; }
}
