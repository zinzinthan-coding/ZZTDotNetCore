using log4net;
using log4net.Config;

namespace ZZTDotNetCore.Log4netRestApi
{
    public static class Log4netExtensions
    {
        public static void AddLog4net(this IServiceCollection services)
        {
            XmlConfigurator.Configure(new FileInfo("log4net.config"));
            services.AddSingleton(LogManager.GetLogger(typeof(Program)));
        }
    }
}
