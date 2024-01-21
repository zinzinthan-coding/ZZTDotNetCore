using Microsoft.EntityFrameworkCore;
using Refit;
using RestSharp;
using Serilog.Sinks.MSSqlServer;
using Serilog;
using System.Reflection;
using ZZTDotNetCore.MvcApp.EFDbContext;
using ZZTDotNetCore.MvcApp.Interfaces;

Log.Logger = new LoggerConfiguration()
.WriteTo.Console()
.WriteTo.File(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs/" + Assembly.GetCallingAssembly().GetName().Name + ".txt"), rollingInterval: RollingInterval.Day, fileSizeLimitBytes: 1024)
.WriteTo.MSSqlServer(
    connectionString: "Server=DESKTOP-TVK5D53\\SQL2022;Database=DotNetCore;User ID=sa;Password=@visible1;TrustServerCertificate=True;",
    sinkOptions: new MSSqlServerSinkOptions
    {
        TableName = "LogEvents",
        AutoCreateSqlTable = true
    })
.CreateLogger();

try
{
    Log.Information("Starting web application");

    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddControllersWithViews();
    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        string? connectionString = builder.Configuration.GetConnectionString("DbConnection");
        options.UseSqlServer(connectionString);
    },
    ServiceLifetime.Transient,
    ServiceLifetime.Transient);

    #region Refit

    builder.Services
        .AddRefitClient<IBlogApi>()
        .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.Configuration.GetSection("ApiUrl").Value!));

    #endregion

    #region HttpClient

    //builder.Services.AddScoped<HttpClient>();
    builder.Services.AddScoped(x => new HttpClient
    {
        BaseAddress = new Uri(builder.Configuration.GetSection("ApiUrl").Value!)
    });

    #endregion

    #region RestClient

    builder.Services.AddScoped(x => new RestClient(builder.Configuration.GetSection("ApiUrl").Value!));

    #endregion

    builder.Host.UseSerilog(); // <-- Add this line

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}