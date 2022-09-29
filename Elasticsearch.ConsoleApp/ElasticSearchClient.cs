using Elasticsearch.Net;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elasticsearch.ConsoleApp
{
    internal class ElasticSearchClient
    {
        public static ElasticClient GetElasticSearchClient()
        {
            var nodes = new Uri[]
            {
                new Uri("http://localhost:9200/"),
            };

            var connectionPool = new StaticConnectionPool(nodes);
            var connectionSettings = new ConnectionSettings(connectionPool).DisableDirectStreaming();
            connectionSettings.BasicAuthentication("elastic", "b=Ey5mn4Vw3Hwp3o1t4i");
            var elasticClient = new ElasticClient(connectionSettings.DefaultIndex("crud_sample"));
            return elasticClient;
        }
    }
}
