using Azure;
using Confluent.Kafka;
using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using N5.Microservices.User.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5.Microservices.User.Infrastructure;
public class ElasticSearchClient: IElasticSearchClient
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

        var values = options.Value;

        var settings = new ElasticsearchClientSettings(new Uri(options.Value.Url))
            .Authentication(new BasicAuthentication(options.Value.User, options.Value.Password));   

        _client = new ElasticsearchClient(settings);

        _logger = logger;
    }

    public ElasticsearchClient Client => _client;

    public async Task<bool> CreateIndex(string index)
    {
        var response = await _client.Indices.CreateAsync(index);

        return response.IsSuccess();
    }

    public async Task<bool> Index(string index, object obj)
    {
        var response = await _client.IndexAsync(obj, index);

        return response.IsSuccess();
    }

    public async Task<T?> Get<T>(string index, int id) where T: class
    {
        var response = await _client.GetAsync<T>(id, idx => idx.Index(index));

        if (response.IsValidResponse)
        {
            return response.Source;
        }

        return null;
    }
}
