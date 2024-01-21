using Microsoft.EntityFrameworkCore;
using ZZTDotNetCore.MinimalApi;
using ZZTDotNetCore.MinimalApi.Features.Blog;
using System.Text.Json.Serialization;
using Serilog;
using System.Reflection;
using Serilog.Sinks.MSSqlServer;

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

	builder.Services.ConfigureHttpJsonOptions(option =>
	{
		option.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
		option.SerializerOptions.PropertyNamingPolicy = null;
	});

	// Add services to the container.
	// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
	builder.Services.AddEndpointsApiExplorer();
	builder.Services.AddSwaggerGen();

	builder.Services.AddDbContext<AppDbContext>(options =>
	{
		string? connectionString = builder.Configuration.GetConnectionString("DbConnection");
		options.UseSqlServer(connectionString);
	},
	ServiceLifetime.Transient,
	ServiceLifetime.Transient);

	builder.Host.UseSerilog(); // <-- Add this line

	var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
	{
		app.UseSwagger();
		app.UseSwaggerUI();
	}

	app.UseHttpsRedirection();

	app.AddBlogService();

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