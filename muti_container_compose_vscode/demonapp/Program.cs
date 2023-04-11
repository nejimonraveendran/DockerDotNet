// dotnet add package Microsoft.Extensions.Hosting --version 7.0.1
// dotnet add package Microsoft.EntityFrameworkCore --version 6.0.15
// dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 6.0.15
// dotnet add package Microsoft.Extensions.Configuration.Json --version 7.0.0

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Dal;
using Microsoft.EntityFrameworkCore;

await Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        string? connectionString = Environment.GetEnvironmentVariable("DBCONNECTION");

        if(string.IsNullOrEmpty(connectionString)){
            connectionString = hostContext.Configuration.GetConnectionString("MyAppDb") ?? string.Empty;
        }

        Console.WriteLine(connectionString);

        services.AddDbContext<MyAppDbContext>(options => options.UseSqlServer(connectionString));
        services.AddHostedService<AppMainService>();
    
    }).RunConsoleAsync();

