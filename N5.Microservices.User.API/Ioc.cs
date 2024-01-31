using Microsoft.EntityFrameworkCore;
using N5.Microservices.User.Infrastructure;

namespace N5.Microservices.User.API;

public static class Ioc
{
    public static void InjectDependencies(this IServiceCollection services)
    {
        services.AddDbContextFactory<EmployeeDBContext>(options => options.UseSqlServer("name=ConnectionStrings:Database"));
    }
}