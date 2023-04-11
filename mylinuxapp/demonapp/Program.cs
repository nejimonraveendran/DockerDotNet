using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using dal;
using demonapp;

await Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        string? connectionString = Environment.GetEnvironmentVariable("DBCONNECTION");

        if (string.IsNullOrEmpty(connectionString))
        {
            connectionString = hostContext.Configuration.GetConnectionString("MyAppDb") ?? string.Empty;
        }

        Console.WriteLine(connectionString);

        services.AddDbContext<MyAppDbContext>(options => options.UseSqlServer(connectionString));
        services.AddHostedService<AppMainService>();

    }).RunConsoleAsync();
