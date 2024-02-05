using Microsoft.EntityFrameworkCore;
using N5.Microservices.User.Domain;

namespace N5.Microservices.User.Infrastructure;
public class EmployeeDbContext : DbContext
{
    public virtual DbSet<Employee> Employees { get; set; }
    public virtual DbSet<Permission> Permissions { get; set; }
    public virtual DbSet<PermissionType> PermissionTypes { get; set; }

    public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options) 
    {
        
    }
}
