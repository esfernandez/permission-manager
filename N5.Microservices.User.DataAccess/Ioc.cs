using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using N5.Microservices.User.DataAccess.Repositories;
using N5.Microservices.User.DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5.Microservices.User.DataAccess;
public static class Ioc
{
    public static void DefineDataAccess(this IServiceCollection services)
    {
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
    }
}
