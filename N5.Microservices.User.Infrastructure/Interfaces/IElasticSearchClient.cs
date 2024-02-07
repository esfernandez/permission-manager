using Elastic.Clients.Elasticsearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5.Microservices.User.Infrastructure.Interfaces;
public interface IElasticSearchClient
{
    Task<IEnumerable<string>> GetIndexes();
    Task<bool> CreateIndex(string index);
    Task<bool> Index(string index, object obj);
    Task<T?> Get<T>(string index, int id) where T : class;

    Task<List<T>> SearchAsync<T, S>(System.Linq.Expressions.Expression<Func<T, S>> exp, string query, string index);

    ElasticsearchClient Client { get; }
}
