using Elasticsearch.WebApp.Models;
using Nest;
using System.Linq.Expressions;

namespace Elasticsearch.WebApp.ElasticSearch
{
    public class ElasticSearchHelper 
    {
        private readonly IElasticClient _client;

        public ElasticSearchHelper(IElasticClient client)
        {
            _client = client;
        }

        public int GetCount<T>() where T : class
        {
           return (int)_client.Count<T>().Count;
        }

        public int GetCountBySearchKeyword<T>(string searchKeyword) where T : class
        {
            return (int)_client.Count<T>(c => c
                                        .Query(q => q
                                        .QueryString(qs => qs
                                        .AnalyzeWildcard()
                                            .Query("*" + searchKeyword.ToLower() + "*")
                                            ))).Count;

        }

        public T GetById<T>(string id) where T : class
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

        public IEnumerable<T> GetAll<T>() where T : class
        {
            return _client.Search<T>(s => s
                                .Query(q => q
                                    .MatchAll()
                                )).Documents;
        }

        public IEnumerable<T> GetAll<T>(int skip, int take) where T : class
        {
            return _client.Search<T>(s => s
                                .Skip(skip)
                                .Size(take)
                                .Query(q => q
                                    .MatchAll()
                                )).Documents;
        }

        public IEnumerable<T> GetByKeywordInMultipleFields<T>(string searchKeyword, int skip, int take) where T : class
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

        public IEnumerable<T> GetByKeywordInMultipleFields<T>(string searchKeyword) where T : class
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
