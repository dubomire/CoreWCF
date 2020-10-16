using CoreWCF.Configuration;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace NetTcpCoreServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseKestrel(options => { options.ListenLocalhost(8080); })
            .UseNetTcp(8080)
            .UseStartup<Startup>();
    }
}
