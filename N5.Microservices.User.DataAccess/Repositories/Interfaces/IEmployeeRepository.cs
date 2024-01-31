using N5.Microservices.User.Domain;

namespace N5.Microservices.User.DataAccess.Repositories.Interfaces;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetAll();
    Task<Employee> GetById(Guid employeeID);
    Task<Employee> Insert(Employee employee);
    Task Update(Employee employee);
    Task Delete(Guid employeeID);
    Task Save();
}
