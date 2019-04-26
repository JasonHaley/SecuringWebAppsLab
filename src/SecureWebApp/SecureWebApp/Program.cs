using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using SecureWebApp.Extensions;

namespace SecureWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseAzureKeyVaultConfiguration()
                .UseStartup<Startup>();
    }
}
