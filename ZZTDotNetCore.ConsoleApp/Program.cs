// See https://aka.ms/new-console-template for more information
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Reflection;
using ZZTDotNetCore.ConsoleApp.AdoDotNetExamples;
using ZZTDotNetCore.ConsoleApp.DapperExamples;
using ZZTDotNetCore.ConsoleApp.EFCoreExamples;
using ZZTDotNetCore.ConsoleApp.HttpClientExamples;
using ZZTDotNetCore.ConsoleApp.RefitExamples;
using ZZTDotNetCore.ConsoleApp.RestClientExamples;

Console.WriteLine("Hello World");

Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File("logs/"+Assembly.GetCallingAssembly().GetName().Name +".txt", rollingInterval: RollingInterval.Day, fileSizeLimitBytes: 1024)
            .WriteTo.MSSqlServer(
                connectionString: "Server=DESKTOP-TVK5D53\\SQL2022;Database=DotNetCore;User ID=sa;Password=@visible1;TrustServerCertificate=True;",
                sinkOptions: new MSSqlServerSinkOptions 
                { 
                    TableName = "LogEvents",
                    AutoCreateSqlTable = true
                })
            .CreateLogger();

//AdoDotNetExample adoDotNetExample= new AdoDotNetExample();
//adoDotNetExample.Run();

//DapperExample dapperExample = new DapperExample();
//dapperExample.Run();

EFCoreExample eFCoreExample = new EFCoreExample();
eFCoreExample.Run();

//Console.WriteLine("Please wait for api...");
//Console.ReadKey();

//HttpClientExample httpClientExample = new HttpClientExample();
//await httpClientExample.Run();

//RestClientExample restClientExample = new RestClientExample();
//await restClientExample.Run();

//RefitExample refitExample = new RefitExample();
//await refitExample.Run();

Console.ReadKey();


