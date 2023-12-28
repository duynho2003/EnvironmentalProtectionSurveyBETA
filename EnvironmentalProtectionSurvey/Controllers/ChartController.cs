using EnvironmentalProtectionSurvey.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
namespace EnvironmentalProtectionSurvey.Controllers
{
    public class ChartController : Controller
    {
        private readonly Project2Context _context;
        private readonly ILogger<HomeController> _logger;
        public ChartController(Project2Context context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Charts()
        {
            return View();
        }
        [HttpPost]
        public List<object> GetList()
        {
            List<object> data = new List<object>();
            // Get survey data from the context
            var surveys = _context.Surveys.ToList();

            // Group surveys by title and count user posts
            var groupedSurveys = surveys.GroupBy(x => x.Title)
                                        .Select(g => new { Title = g.Key, TotalUserPosts = g.Sum(x => x.UserPost) })
                                        .ToList();

            // Prepare data for the chart
            List<string?> labels = groupedSurveys.Select(x => x.Title).ToList();
            List<int?> totals = groupedSurveys.Select(x => x.TotalUserPosts).ToList();
            data.Add(labels);
            data.Add(totals);
            return data;
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
