using Elasticsearch.WebApp.Data;
using Elasticsearch.WebApp.ElasticSearch;
using Elasticsearch.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Nest;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System.Data;
using System.Diagnostics;

namespace Elasticsearch.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IElasticClient _client;
        private readonly ElasticSearchHelper<ProductDetails> _elasticHelper;
        private Products results = new Products();

        public HomeController(ILogger<HomeController> logger, IElasticClient client)
        {
            _logger = logger;
            _client = client;
            _elasticHelper = new ElasticSearchHelper<ProductDetails>(client);
        }

        public IActionResult Index()
        {
            Func<SortDescriptor<ProductDetails>, IPromise<IList<ISort>>> Sorting;
            Sorting = sort => sort.Ascending(f => f.ProductName.Suffix("keyword"));
            results.Product = _elasticHelper.GetAll(0, results.PageSize);
            results.Product = _elasticHelper.GetAllDataWithSorting(0, results.PageSize, Sorting);
            double pageCount = (double)(_elasticHelper.GetCount() / Convert.ToDecimal(results.PageSize));
            results.PageCount = (int)Math.Ceiling(pageCount);
            return View(results);
        }

        //[HttpPost]
        public IActionResult GetProduct(string searchKeyword, string sortBy, string sortOrder, int pageIndex = 1, string pageSize = "5")
        {
            var skip = (pageIndex - 1) * results.PageSize;
            int totalResultFound;


            results.PageSize = int.Parse(pageSize);
            if (sortOrder == "ASC")
            {
                results.SortOrder = "DESC";
            }
            else
            {
                results.SortOrder = "ASC";
            }
            Func<SortDescriptor<ProductDetails>, IPromise<IList<ISort>>> Sorting;
            switch (sortBy, sortOrder)
            {
                case ("Price", "ASC"):
                    Sorting = sort => sort.Ascending(f => f.Price);
                    break;
                case ("Price", "DESC"):
                    Sorting = sort => sort.Descending(f => f.Price);
                    break;
                case ("Name", "DESC"):
                    Sorting = sort => sort.Descending(f => f.ProductName.Suffix("keyword"));
                    break;
                case ("Name", "ASC"):
                default:
                    Sorting = sort => sort.Ascending(f => f.ProductName.Suffix("keyword"));
                    break;
            }

            //results.SortOrder = sortOrder;

            if (string.IsNullOrEmpty(searchKeyword))
            {
                results.Product = _elasticHelper.GetAllDataWithSorting(skip, results.PageSize, Sorting);
                totalResultFound = _elasticHelper.GetCount();
            }
            else
            {
                searchKeyword = searchKeyword.Trim();
                //results.Product = _elasticHelper.GetByKeywordInMultipleFields(searchKeyword, skip, results.PageSize);
                results.Product = _elasticHelper.GetByKeywordInMultipleFieldsWithSorting(searchKeyword, skip, results.PageSize, Sorting);
                totalResultFound = _elasticHelper.GetCountBySearchKeyword(searchKeyword);
            }

            double pageCount = (double)(totalResultFound / Convert.ToDecimal(results.PageSize));
            results.PageCount = (int)Math.Ceiling(pageCount);

            results.PageIndex = pageIndex;

            results.SearchKeyword = searchKeyword;

            results.PageSizes.Where(v => v.Value == pageSize).First().Selected = true;

            return View("Index", results);
        }

        public IActionResult ExportToExcel()
        {
            var exportbytes = ExporttoExcel(_elasticHelper.GetAll());
            return File(exportbytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Report.xlsx");
        }

        private byte[] ExporttoExcel<T>(IEnumerable<T> data)
        {
            using ExcelPackage pack = new ExcelPackage();
            ExcelWorksheet ws = pack.Workbook.Worksheets.Add("Sheet1");
            ws.Cells.LoadFromCollection(data, true, TableStyles.Light9);
            return pack.GetAsByteArray();
        }


        public IActionResult AddProduct()
        {
            var productDetails = new ProductVM
            {
                ProductId = Guid.NewGuid(),
                Colors = MasterData.GetColors(),
                Tags = MasterData.GetTags()
            };

            return View(productDetails);
        }

        public IActionResult EditProduct(string id)
        {

            var results = _elasticHelper.GetById(id);

            var colors = MasterData.GetColors();
            var tags = MasterData.GetTags();

            foreach (var item in results.Colors)
            {
                for (int i = 0; i < colors.Count; i++)
                {
                    if (item == colors[i].Text)
                    {
                        colors[i].IsChecked = true;
                    }
                }
            }

            foreach (var item in results.Tags)
            {
                for (int i = 0; i < tags.Count; i++)
                {
                    if (item == tags[i].Text)
                    {
                        tags[i].IsChecked = true;
                    }
                }
            }

            var productDetails = new ProductVM
            {
                ProductId = results.Id,
                ProductName = results.ProductName,
                Price = results.Price,
                Description = results.Description,
                Colors = colors,
                Tags = tags
            };

            return View("AddProduct", productDetails);
        }

        [HttpPost]
        public IActionResult AddProduct(ProductVM product)
        {
            var productDetail = new ProductDetails()
            {
                Id = product.ProductId,
                ProductName = product.ProductName,
                Price = product.Price,
                Description = product.Description,
                Colors = product.Colors.Where(x => x.IsChecked == true).Select(x => x.Text).ToArray(),
                Tags = product.Tags.Where(x => x.IsChecked == true).Select(x => x.Text).ToArray()

            };

            var indexResponse1 = _client.IndexDocument(productDetail);
            return RedirectToAction("AddProduct");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}