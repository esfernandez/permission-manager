using N5.Microservices.User.Application;
using N5.Microservices.User.DataAccess;
using N5.Microservices.User.Infrastructure;

namespace N5.Microservices.User.API;

public static class Ioc
{
    public static void InjectDependencies(this IServiceCollection services)
    {
        services.DefineDataAccess();
        services.DefineInfrastructure();
        services.DefineApplication();
    }
}