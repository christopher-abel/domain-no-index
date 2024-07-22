using DomainNoIndex.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DomainNoIndex.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("/robots.txt")]
        public IActionResult Robots()
        {
            var content = "User-agent: *\n" +
                "Disallow: /";
            return Content(content, "text/plain");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
