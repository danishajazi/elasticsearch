namespace Elasticsearch.WebApp.Models
{
    public class Products : Pager
    {
        public IEnumerable<ProductDetails> Product { get; set; }
        public string SearchKeyword { get; set; }
    }
}
