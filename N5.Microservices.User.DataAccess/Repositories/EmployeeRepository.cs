using Microsoft.EntityFrameworkCore;
using N5.Microservices.User.DataAccess.Repositories.Interfaces;
using N5.Microservices.User.Domain;

namespace N5.Microservices.User.DataAccess.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private bool disposed = false;
    private readonly EmployeeDBContext _context;
    public EmployeeRepository()
    {
        _context = new EmployeeDBContext();
    }
    
    public EmployeeRepository(EmployeeDBContext context)
    {
        _context = context;
    }
    
    public IEnumerable<Employee> GetAll()
    {
        return _context.Employees.ToList();
    }
    
    public Employee GetById(int EmployeeID)
    {
        return _context.Employees.Find(EmployeeID);
    }
    
    public void Insert(Employee employee)
    {
        _context.Employees.Add(employee);
    }
    
    public void Update(Employee employee)
    {
        _context.Entry(employee).State = EntityState.Modified;
    }
    
    public void Delete(int EmployeeID)
    {
        Employee employee = _context.Employees.Find(EmployeeID);
        if (employee != null)
        {
            _context.Employees.Remove(employee);
        }
    }
    
    public void Save()
    {
        _context.SaveChanges();
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