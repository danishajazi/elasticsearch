using Microsoft.AspNetCore.Mvc.Rendering;

namespace Elasticsearch.WebApp.Models
{
    public class Pager
    {
        public Pager()
        {
            PageSizes = new List<SelectListItem>
            {
                new SelectListItem {Text="5", Value="5"},
                new SelectListItem {Text="10", Value="10"},
                new SelectListItem {Text="15", Value="15"},
                new SelectListItem {Text="20", Value="20"},
            };
        }
        public int PageIndex { get; set; } = 1;
        public int PageCount { get; set; }
        public int PageSize { get; set; } = 5;
        public List<SelectListItem> PageSizes { get; set; }

        public string SortOrder { get; set; } = "ASC";
    }

    //public class Itemlist
    //{
    //    public string Text { get; set; }
    //    public int Value { get; set; }
    //}
}
