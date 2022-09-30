using Elasticsearch.Net;
using Nest;

namespace Elasticsearch.WebApp.ElasticSearch
{
    public class ElasticSearchClient
    {
        public static IElasticClient GetElasticSearchClient()
        {
            var nodes = new Uri[]
             {
                new Uri("http://localhost:9200/"),
             };

            var connectionPool = new StaticConnectionPool(nodes);
            var connectionSettings = new ConnectionSettings(connectionPool).DisableDirectStreaming();
            connectionSettings.BasicAuthentication("elastic", "b=Ey5mn4Vw3Hwp3o1t4i");
            var elasticClient = new ElasticClient(connectionSettings.DefaultIndex("productdetails"));
            return elasticClient;
        }
    }
}
