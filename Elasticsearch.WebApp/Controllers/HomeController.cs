using Elasticsearch.WebApp.Data;
using Elasticsearch.WebApp.ElasticSearch;
using Elasticsearch.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Nest;
using System.Diagnostics;

namespace Elasticsearch.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IElasticClient _client;
        private readonly ElasticSearchHelper _elasticHelper;
        private Products results = new Products();

        public HomeController(ILogger<HomeController> logger, IElasticClient client)
        {
            _logger = logger;
            _client = client;
            _elasticHelper = new ElasticSearchHelper(client);
        }

        public IActionResult Index()
        {
            results.Product = _elasticHelper.GetAll<ProductDetails>(0, results.PageSize);
            double pageCount = (double)(_elasticHelper.GetCount<ProductDetails>() / Convert.ToDecimal(results.PageSize));
            results.PageCount = (int)Math.Ceiling(pageCount);
            return View(results);
        }

        [HttpPost]
        public IActionResult Index(string searchKeyword, int pageIndex = 1)
        {
            var skip = (pageIndex - 1) * results.PageSize;
            int totalResultFound;
            

            if (string.IsNullOrEmpty(searchKeyword))
            {
                results.Product = _elasticHelper.GetAll<ProductDetails>(skip, results.PageSize);
                totalResultFound = _elasticHelper.GetCount<ProductDetails>();
            }
            else
            {
                searchKeyword = searchKeyword.Trim();
                results.Product = _elasticHelper.GetByKeywordInMultipleFields<ProductDetails>(searchKeyword, skip, results.PageSize);
                totalResultFound = _elasticHelper.GetCountBySearchKeyword<ProductDetails>(searchKeyword);
            }

            double pageCount = (double)(totalResultFound / Convert.ToDecimal(results.PageSize));
            results.PageCount = (int)Math.Ceiling(pageCount);

            results.PageIndex = pageIndex;

            results.SearchKeyword = searchKeyword;

            return View(results);
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

            var results = _elasticHelper.GetById<ProductDetails>(id);

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