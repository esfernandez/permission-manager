using N5.Microservices.User.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5.Microservices.User.DataAccess.Repositories.Interfaces;
public interface IPermissionRepository
{
    Task<Permission> RequestPermission(Permission permission);
    Task UpdatePermission(Permission permission);
    Task<IEnumerable<Permission>> GetPermissions(Employee employee);
    Task SyncPermissions(Employee employee);
    Task Save();
}
