using Microsoft.EntityFrameworkCore;
using N5.Microservices.User.DataAccess;
using N5.Microservices.User.Infrastructure;
using N5.Microservices.User.Infrastructure.Interfaces;

namespace N5.Microservices.User.API;

public static class Ioc
{
    public static void InjectDependencies(this IServiceCollection services)
    {
        services.DefineDataAccess();
        services.DefineInfrastructure();
    }
}