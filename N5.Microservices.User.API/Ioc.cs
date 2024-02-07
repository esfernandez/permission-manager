using N5.Microservices.User.API.Services;
using N5.Microservices.User.Application;
using N5.Microservices.User.Application.DTOs;
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

        services.DefineEventConsumer<PermissionEventDto>();

        services.AddHostedService<PermissionConsumerService>();
    }
}