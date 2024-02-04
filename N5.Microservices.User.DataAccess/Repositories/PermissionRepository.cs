using Confluent.Kafka;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using N5.Microservices.User.DataAccess.Repositories.Interfaces;
using N5.Microservices.User.Domain;
using N5.Microservices.User.Domain.Abstractions;
using N5.Microservices.User.Infrastructure;
using N5.Microservices.User.Infrastructure.Interfaces;
using System.Security;
using System.Threading;

namespace N5.Microservices.User.DataAccess.Repositories;
public class PermissionRepository : IPermissionRepository
{
    private readonly EmployeeDbContext _dbContext;
    private readonly IElasticSearchClient _elasticSearchClient;

    private const string INDEX_ELASTIC = "permissions";
    public PermissionRepository(EmployeeDbContext dbContext, IElasticSearchClient elasticSearchClient)
    {
        _dbContext = dbContext;
        _elasticSearchClient = elasticSearchClient;
    }

    public async Task<IEnumerable<Permission>> GetPermissions(Employee employee)
    {
        var searchResponse = await _elasticSearchClient.Client.SearchAsync<Permission>(s => s
        .Query(q => q
            .Match(m => m
                .Field(f => f.Employee.Id)
                .Query(employee.Id.ToString())  // Reemplaza "1" con el ID del Employee específico
            )
        ));

        if (searchResponse.IsValidResponse)
        {
            return searchResponse.Documents.ToList();
        }
        else
        {
            throw new ArgumentException($"Ocurrió un error al intentar obtener los permisos del empleado {employee.Id}");
        }
    }

    public async Task<Permission> RequestPermission(Permission permission)
    {
        await _dbContext.AddAsync(permission);
        return permission;
    }

    public Task UpdatePermission(Permission permission)
    {
        _dbContext.Update(permission);

        return Task.CompletedTask;
    }

    public async Task SyncPermissions(Employee employee)
    {
        var response = await _elasticSearchClient.Client.SearchAsync<Permission>();

        var queryContainer = new TermQuery(new Field("employee.id"))
        {
            Value = employee.Id.ToString()
        };

        var searchRequest = new DeleteByQueryRequestDescriptor<Permission>(INDEX_ELASTIC).Query(queryContainer);

        var deleteByQueryResponse = _elasticSearchClient.Client.DeleteByQuery<Permission>(searchRequest);

        if (response.IsValidResponse)
        {
            await _elasticSearchClient.Client.BulkAsync(b => b.Index(INDEX_ELASTIC).IndexMany(employee.Permissions));
        }
    }
}
