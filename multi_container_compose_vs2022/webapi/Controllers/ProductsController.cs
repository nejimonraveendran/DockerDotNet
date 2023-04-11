using dal;
using dal.Entities;
using Microsoft.AspNetCore.Mvc;

namespace webapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {

        private readonly ILogger<ProductsController> _logger;
        private readonly MyAppDbContext myAppDbContext;

        public ProductsController(ILogger<ProductsController> logger, MyAppDbContext myAppDbContext)
        {
            _logger = logger;
            this.myAppDbContext = myAppDbContext;
        }

        [HttpGet(Name = "Products")]
        public IEnumerable<Product>? Get()
        {
            return myAppDbContext.Products?.ToList();
        }

        [HttpPost(Name = "Products")]
        public void Post([FromBody] Product product)
        {
            myAppDbContext.Products.Add(new Product { ProductName = product.ProductName });
            myAppDbContext.SaveChanges();
        }
    }
}