using Dal;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class AppMainService : IHostedService
{
    private readonly ILogger<AppMainService> _logger;
    private readonly IHostApplicationLifetime _appLifetime;
    private readonly MyAppDbContext _myAppDbContext;

    public AppMainService(ILogger<AppMainService> logger, 
    IHostApplicationLifetime appLifetime, MyAppDbContext myAppDbContext)
    {
        _logger = logger;
        _appLifetime = appLifetime;
        _myAppDbContext = myAppDbContext;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("App starting...");
        _logger.LogInformation("Getting products from DB...");

        try
        {
            var products = _myAppDbContext.Products?.ToList();

            foreach (var item in products)
            {
                Console.WriteLine($"{item.Id} - {item.ProductName}");
            }    
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex.Message);        
        }
        
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("App stopping...");
        return Task.CompletedTask;
    }
}
