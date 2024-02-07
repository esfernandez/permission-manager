using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.IndexManagement;
using Elastic.Transport;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using N5.Microservices.User.Domain;
using N5.Microservices.User.Infrastructure.Interfaces;
using System.Linq.Expressions;

namespace N5.Microservices.User.Infrastructure;
public class ElasticSearchClient : IElasticSearchClient
{
    private readonly ElasticsearchClient _client;
    private readonly ILogger<ElasticSearchClient> _logger;

    public ElasticSearchClient(IOptions<ElasticsearchOptions> options, ILogger<ElasticSearchClient> logger)
    {
        ArgumentNullException.ThrowIfNull(options.Value);

        ArgumentException.ThrowIfNullOrEmpty(options.Value.Url);
        ArgumentException.ThrowIfNullOrEmpty(options.Value.DefaultIndex);
        ArgumentException.ThrowIfNullOrEmpty(options.Value.User);
        ArgumentException.ThrowIfNullOrEmpty(options.Value.Password);

        var settings = new ElasticsearchClientSettings(new Uri(options.Value.Url))
            .Authentication(new BasicAuthentication(options.Value.User, options.Value.Password))
            .ServerCertificateValidationCallback((x, y, z, q) => true);

        _client = new ElasticsearchClient(settings);

        _logger = logger;
    }

    public ElasticsearchClient Client => _client;

    public async Task<bool> CreateIndex(string index)
    {
        var response = await _client.Indices.CreateAsync(index);

        if (!response.IsValidResponse)
        {
            response.TryGetOriginalException(out var ex);
            throw new Exception("No se pudo crear el índice en el servicio de ElasticSearch", ex);
        }

        return response.IsSuccess();
    }

    public async Task<IEnumerable<string>> GetIndexes()
    {
        var response = await _client.Indices.GetAsync(new GetIndexRequest(Indices.All));

        if (!response.IsValidResponse)
        {
            response.TryGetOriginalException(out var ex);
            throw new Exception("No se pudo leer los índices del servicio de ElasticSearch",ex);
        }

        var indices = response.Indices.ToList();

        return indices.Select(x => x.Key.ToString());
    }

    public async Task<bool> Index(string index, object obj)
    {
        var response = await _client.IndexAsync(obj, index);

        return response.IsSuccess();
    }

    public async Task<T?> Get<T>(string index, int id) where T : class
    {
        var response = await _client.GetAsync<T>(id, idx => idx.Index(index));

        if (response.IsValidResponse)
        {
            return response.Source;
        }

        return null;
    }

    public async Task<List<T>> SearchAsync<T, S>(System.Linq.Expressions.Expression<Func<T, S>> exp, string query, string index)
    {
        var searchResponse = await Client.SearchAsync<T>(s => s
            .Index(index)
            .Query(q => q
                .Match(m => m
                    .Field(exp)
                    .Query(query))
                )
            );

        if (searchResponse.IsValidResponse)
        {
            return searchResponse.Documents.ToList();
        }

        searchResponse.TryGetOriginalException(out var ex);

        throw new Exception("Error at search to ElasticSearch server.", ex);
    }
}
