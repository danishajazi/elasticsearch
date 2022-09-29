using Elasticsearch.ConsoleApp.Entities;
using Nest;

namespace Elasticsearch.ConsoleApp
{
    internal class Search
    {
        private ElasticClient _client;
        public Search(ElasticClient client)
        {
            _client = client;
        }
        public ISearchResponse<Customer> GetAll()
        {
            return _client.Search<Customer>(s => s
                                .Query(q => q
                                    .MatchAll()
                                ));
        }

        public ISearchResponse<Customer> GetAllBySearchText(string searchKeyword)
        {
            return _client.Search<Customer>(s => s
                    .Query(q => q
                        .Match(m => m
                        .Field(f => f.Name)
                        .Query(searchKeyword)
                        )
                    ));
        }

        public ISearchResponse<Customer> GetByWildcardSearchOnOneFiled(string searchKeyword)
        {
            return _client.Search<Customer>(s => s
                                .Query(q => q
                                .QueryString(qs => qs
                                   .Query("*" + searchKeyword + "*")
                                   .Fields(fs => fs
                                       .Fields(f1 => f1.Name)
                                   )
                                   )));
        }

        public ISearchResponse<Customer> GetByWildcardSearchInMultipleFileds(string searchKeyword)
        {
            return _client.Search<Customer>(s => s
                                .Query(q => q
                                .QueryString(qs => qs
                                   .Query("*" + searchKeyword + "*")
                                   .Fields(fs => fs
                                       .Fields(
                                            f1 => f1.Name,
                                            f2 => f2.Address,
                                            f3 => f3.Orders
                                            )
                                         )
                                   )));
        }

        public ISearchResponse<Customer> GetAllDataWithPaging(int skip = 5, int take = 5)
        {
            ISearchResponse<Customer> results;

            /*There are two ways for paging.*/

            //This is first approach
            results = _client.Search<Customer>(s => s
                    .Query(q => q
                        .MatchAll()
                        )
                        .From(skip)
                        .Size(take)
                    );

            //This is second approach
            results = _client.Search<Customer>(s => s
                    .From(skip)
                    .Size(take)
                    .Query(q => q
                        .MatchAll()
                    ));
            return results;
        }

        public ISearchResponse<Customer> GetAllDataWithSorting()
        {
            ISearchResponse<Customer> results = _client.Search<Customer>(s => s
                        .Query(q => q
                            .MatchAll()
                        )
                       .Sort(sort => sort.Ascending(f => f.Name.Suffix("keyword")))
                    );
            return results;
        }

    }
}
