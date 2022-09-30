using Nest;

namespace Elasticsearch.WebApp.ElasticSearch
{
    public class ElasticSearchHelper
    {
        private readonly IElasticClient _client;

        public ElasticSearchHelper(IElasticClient client)
        {
            _client = client;
        }

        public IEnumerable<T> GetAll<T>() where T : class
        {
            return _client.Search<T>(s => s
                                .Query(q => q
                                    .MatchAll()
                                )).Documents;
        }
    }
}
