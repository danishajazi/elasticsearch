using Elasticsearch.WebApp.Data;
using Elasticsearch.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Nest;
using System.Diagnostics;

namespace Elasticsearch.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ElasticClient _client;

        public HomeController(ILogger<HomeController> logger, ElasticClient client)
        {
            _logger = logger;
            _client = client;
        }

        public IActionResult Index()
        {
            ISearchResponse<ProductDetails> results = _client.Search<ProductDetails>(s => s
                                .Query(q => q
                                    .MatchAll()
                                ));

            List<ProductDetails> products = results.Documents.ToList();

            return View(products);
        }

        [HttpPost]
        public IActionResult Index(string searchKeyword)
        {
            ISearchResponse<ProductDetails> results;

            if (string.IsNullOrEmpty(searchKeyword))
            {
                results = _client.Search<ProductDetails>(s => s
                                .Query(q => q
                                    .MatchAll()
                                ));
            }
            else
            {
                results = _client.Search<ProductDetails>(s => s.Source()
                                    .Query(q => q
                                    .QueryString(qs => qs
                                    .AnalyzeWildcard()
                                       .Query("*" + searchKeyword.ToLower() + "*")
                                       .Fields(fs => fs
                                           .Fields(f1 => f1.ProductName,
                                                   f2 => f2.Description,
                                                   f3 => f3.Tags,
                                                   f4 => f4.Colors
                                                   )
                                       )
                                       )));
            }
            List<ProductDetails> products = results.Documents.ToList();
            return View(products);
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
            var results = _client.Search<ProductDetails>(s => s
                                .Query(q => q
                                    .MatchAll()
                                )).Documents.Where(w => w.Id == Guid.Parse(id)).FirstOrDefault();

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
                ProductId= results.Id,
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