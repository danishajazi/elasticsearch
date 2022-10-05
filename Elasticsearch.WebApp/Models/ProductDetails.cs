namespace Elasticsearch.WebApp.Models
{
    public class ProductDetails
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string[] Colors { get; set; }
        public string[] Tags { get; set; }
    }
}
