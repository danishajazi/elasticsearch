namespace Elasticsearch.WebApp.Models
{
    public class ProductVM
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public List<CheckBoxModel> Colors { get; set; }
        public List<CheckBoxModel> Tags { get; set; }
    }

    public class CheckBoxModel
    {
        public int Value { get; set; }
        public string Text { get; set; }
        public bool IsChecked { get; set; }
    }
}
