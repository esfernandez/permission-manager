using Microsoft.EntityFrameworkCore;
using N5.Microservices.User.DataAccess.Repositories.Interfaces;
using N5.Microservices.User.Domain;
using N5.Microservices.User.Infrastructure;
namespace N5.Microservices.User.DataAccess.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private bool disposed = false;
    private readonly EmployeeDbContext _context;
    public EmployeeRepository(EmployeeDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Employee>> GetAll()
    {
        return await _context.Employees.ToListAsync();
    }
    
    public async Task<Employee?> GetById(Guid EmployeeID)
    {
        return await _context.Employees.Include(x => x.Permissions).FirstOrDefaultAsync(x => x.Id == EmployeeID);
    }
    
    public async Task<Employee> Insert(Employee employee)
    {
        await _context.Employees.AddAsync(employee);
        return employee;
    }
    
    public void Update(Employee employee)
    {
        _context.Entry(employee).State = EntityState.Modified;
    }
    
    public async Task Delete(Guid id)
    {
        Employee? employee = await _context.Employees.FindAsync(id);
        if (employee != null)
        {
            _context.Employees.Remove(employee);
        }
    }
    
    public async Task Save()
    {
        await _context.SaveChangesAsync();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}