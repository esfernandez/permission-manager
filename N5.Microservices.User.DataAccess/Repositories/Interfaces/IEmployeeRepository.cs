using N5.Microservices.User.Domain;

namespace N5.Microservices.User.DataAccess.Repositories.Interfaces;

public interface IEmployeeRepository
{
    IEnumerable<Employee> GetAll();
    Employee GetById(int EmployeeID);
    void Insert(Employee employee);
    void Update(Employee employee);
    void Delete(int EmployeeID);
    void Save();
}
