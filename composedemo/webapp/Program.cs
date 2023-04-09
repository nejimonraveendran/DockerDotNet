using Dal;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

string connectionString = Environment.GetEnvironmentVariable("DBCONNECTION") ?? string.Empty;

if(string.IsNullOrEmpty(connectionString)){
    connectionString = builder.Configuration.GetConnectionString("MyAppDb");
}

Console.WriteLine(connectionString);

builder.Services.AddDbContext<MyAppDbContext>(options => options.UseSqlServer(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    Console.WriteLine("Prod mode enabled");
    app.UseExceptionHandler("/Home/Error");
}else{
    Console.WriteLine("Development mode enabled");
}


app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
