using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using N5.Microservices.User.Domain;
using N5.Microservices.User.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5.Microservices.User.Infrastructure;
public static class Ioc
{
    public static void DefineInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<EmployeeDbContext>(options => options.UseSqlServer("name=ConnectionStrings:Database", options => 
        { 
            options.EnableRetryOnFailure();
            options.MigrationsAssembly("N5.Microservices.User.API");
        }));

        services.AddSingleton<IElasticSearchClient, ElasticSearchClient>();

        services.AddSingleton<IEventConsumer<Permission>, KafkaConsumerService<Permission>>();
        services.AddSingleton<IEventProducer, KafkaProducer>();
    }

    public static void UseInfrastructure(this IServiceProvider service)
    {
        using var scope = service.CreateScope();
        using var context = scope.ServiceProvider.GetService<EmployeeDbContext>();
        context.Database.EnsureCreated();
    }
}