using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ViewDiscordBot.Models;

namespace ViewDiscordBot.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index() => View();

        public IActionResult Command() => View();

        public IActionResult SignUp()
        {
            return View(new { Text = "This is Authoriation" });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext?.TraceIdentifier });
        }
    }
}