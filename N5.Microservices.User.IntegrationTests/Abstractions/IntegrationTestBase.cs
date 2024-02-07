using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using N5.Microservices.User.Application;
using N5.Microservices.User.DataAccess;
using N5.Microservices.User.Domain;
using N5.Microservices.User.Infrastructure.Interfaces;
using N5.Microservices.User.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Elastic.Clients.Elasticsearch;
using Elastic.Transport.Products.Elasticsearch;
using System.Linq.Expressions;

namespace N5.Microservices.User.IntegrationTests.Abstractions;
public class IntegrationTestBase : IDisposable
{
    protected readonly IServiceProvider ServiceProvider;

    protected Mock<IElasticSearchClient> ElasticSearchClientMock;

    public IntegrationTestBase()
    {
        var services = new ServiceCollection();        
        
        services.DefineApplication();
        services.DefineDataAccess();

        var elasticMock = new Mock<IElasticSearchClient>();

        var searchResponse = new SearchResponse<Permission>();

        var eventProducerMock = new Mock<IEventProducer>();
        var eventConsumerMock = new Mock<IEventConsumer<Permission>>();

        services.AddSingleton(elasticMock.Object);
        services.AddSingleton(eventConsumerMock.Object);
        services.AddSingleton(eventProducerMock.Object);

        services.AddControllers();
        services.AddLogging();

        var dbContextOptions = new DbContextOptionsBuilder<EmployeeDbContext>()
                                .UseInMemoryDatabase(databaseName: "TestDatabase")
                                .Options;

        services.AddScoped((_) => new EmployeeDbContext(dbContextOptions));

        ServiceProvider = services.BuildServiceProvider();
    }

    public void Dispose()
    {
        // Realiza la limpieza de recursos si es necesario
        if (ServiceProvider is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}