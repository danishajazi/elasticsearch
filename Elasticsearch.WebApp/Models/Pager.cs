namespace Elasticsearch.WebApp.Models
{
    public class Pager
    {
        public int PageIndex { get; set; } = 1;
        public int PageCount { get; set; }
        public int PageSize { get; set; } = 5;
    }
}
