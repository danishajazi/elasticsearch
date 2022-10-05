using Elasticsearch.WebApp.Models;
using Nest;

namespace Elasticsearch.WebApp.ElasticSearch
{
    public class ElasticSearchHelper<T> where T : class
    {
        private readonly IElasticClient _client;

        public ElasticSearchHelper(IElasticClient client)
        {
            _client = client;
        }

        public int GetCount()
        {
            return (int)_client.Count<T>().Count;
        }

        public int GetCountBySearchKeyword(string searchKeyword)
        {
            return (int)_client.Count<T>(c => c
                                        .Query(q => q
                                        .QueryString(qs => qs
                                        .AnalyzeWildcard()
                                            .Query("*" + searchKeyword.ToLower() + "*")
                                            ))).Count;

        }

        public T GetById(string id)
        {
            var result = _client.Get<T>(id).Source;

            //There are multiple ways to fecth data based on id
            //var result = _client.Source<T>(id).Body;

            //Following methods will fecth data based on multiple id i.e. array id ids
            //var result = _client.Search<T>(s => s
            //                    .Query(q => q
            //                            .Ids(i => i.Values(new string[] { id }))));

            //var result = _client.Search<T>(s => s
            //                    .Query(q => q
            //                        .Bool(b => b
            //                            .Must(m => m
            //                                .Ids(i => i.Values(new string[] { id })))))).Documents.FirstOrDefault();

            return result;
        }

        public IEnumerable<T> GetAll()
        {
            return _client.Search<T>(s => s
                                .Query(q => q
                                    .MatchAll()
                                )).Documents;
        }

        public IEnumerable<T> GetAllDataWithSorting(int skip, int take, 
            Func<SortDescriptor<T>, IPromise<IList<ISort>>> sorting)
        {
            var results = _client.Search<T>(s => s
                                        .Skip(skip)
                                        .Size(take)
                                        .Query(q => q
                                            .MatchAll()
                                            )
                                            .Sort(sorting)
                                        ).Documents;
            return results;
        }

        public IEnumerable<T> GetAll(int skip, int take)
        {
            return _client.Search<T>(s => s
                                .Skip(skip)
                                .Size(take)
                                .Query(q => q
                                    .MatchAll()
                                )).Documents;
        }

        public IEnumerable<T> GetByKeywordInMultipleFieldsWithSorting(string searchKeyword, int skip, int take,
            Func<SortDescriptor<T>, IPromise<IList<ISort>>> sorting)
        {
            return _client.Search<T>(s => s.Source()
                                .Skip(skip)
                                .Size(take)
                                .Query(q => q
                                .QueryString(qs => qs
                                .AnalyzeWildcard()
                                   .Query("*" + searchKeyword.ToLower() + "*")
                                   )).Sort(sorting)).Documents;
        }

        public IEnumerable<T> GetByKeywordInMultipleFields(string searchKeyword, int skip, int take)
        {
            return _client.Search<T>(s => s.Source()
                                .Skip(skip)
                                .Size(take)
                                .Query(q => q
                                .QueryString(qs => qs
                                .AnalyzeWildcard()
                                   .Query("*" + searchKeyword.ToLower() + "*")
                                   ))).Documents;

            //return _client.Search<ProductDetails>(s => s.Source()
            //                    .Query(q => q
            //                    .QueryString(qs => qs
            //                    .AnalyzeWildcard()
            //                       .Query("*" + searchKeyword.ToLower() + "*")
            //                       .Fields(fs => fs
            //                           .Fields(f1 => f1.ProductName,
            //                                   f2 => f2.Description,
            //                                   f3 => f3.Tags,
            //                                   f4 => f4.Colors
            //                                   )
            //                       )
            //                       ))).Documents;

        }

        public IEnumerable<T> GetByKeywordInMultipleFields(string searchKeyword)
        {
            return _client.Search<T>(s => s.Source()
                                .Query(q => q
                                .QueryString(qs => qs
                                .AnalyzeWildcard()
                                   .Query("*" + searchKeyword.ToLower() + "*")
                                   ))).Documents;

            //return _client.Search<ProductDetails>(s => s.Source()
            //                    .Query(q => q
            //                    .QueryString(qs => qs
            //                    .AnalyzeWildcard()
            //                       .Query("*" + searchKeyword.ToLower() + "*")
            //                       .Fields(fs => fs
            //                           .Fields(f1 => f1.ProductName,
            //                                   f2 => f2.Description,
            //                                   f3 => f3.Tags,
            //                                   f4 => f4.Colors
            //                                   )
            //                       )
            //                       ))).Documents;

        }

    }
}
