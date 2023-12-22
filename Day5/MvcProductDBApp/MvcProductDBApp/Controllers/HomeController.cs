using Microsoft.AspNetCore.Mvc;
using MvcProductDBApp.Models;
using MvcProductDBApp.Services;
using System.Diagnostics;

namespace MvcProductDBApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

            _logger.LogWarning("WARNING:Inside home controller");
        }

        public IActionResult Index()
        {
            _logger.LogWarning("WARNING:Inside home controller Index");

            ProductService productService = new ProductService();
            ViewBag.Products = productService.GetProducts();

            return View();
        }

        public IActionResult Privacy()
        {
            _logger.LogWarning("WARNING:Inside home controller Privacy");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            _logger.LogWarning("WARNING:Inside home controller error");
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
