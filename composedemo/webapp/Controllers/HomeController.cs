using System.Diagnostics;
using Dal;
using Microsoft.AspNetCore.Mvc;
using webapp.Models;

namespace webapp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly MyAppDbContext _myAppDbContext;

    public HomeController(ILogger<HomeController> logger, MyAppDbContext myAppDbContext)
    {
        _logger = logger;
        _myAppDbContext = myAppDbContext;
    }

    public IActionResult Index()
    {
        _logger.LogInformation("Home page accessed");

        var users = _myAppDbContext.Users?.ToList();

        _logger.LogInformation(users?.Count.ToString());

        return View();
    }

    public IActionResult Privacy()
    {
        return View();

        
    }

    public string health()
    {
        string value = "good1";
        return value;
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
