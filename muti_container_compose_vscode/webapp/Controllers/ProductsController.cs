using System.Diagnostics;
using Dal;
using Microsoft.AspNetCore.Mvc;
using webapp.Models;

namespace webapp.Controllers;

public class ProductsController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly MyAppDbContext _myAppDbContext;

    public ProductsController(ILogger<HomeController> logger, MyAppDbContext myAppDbContext)
    {
        _logger = logger;
        _myAppDbContext = myAppDbContext;
    }

    public IActionResult Index()
    {
        _logger.LogInformation("Products page accessed");

        var products = _myAppDbContext.Products?.Select(x => new webapp.Models.ProductViewModel 
        { 
            Id = x.Id, 
            ProductName = x.ProductName 
        }).ToList();

        return View(products);
    }

}
