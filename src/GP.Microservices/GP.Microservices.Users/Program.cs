using Microsoft.AspNetCore.Hosting;

namespace GP.Microservices.Users
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            new WebHostBuilder()
                .UseKestrel()
                .UseStartup<Startup>()
                .UseIISIntegration()
                .Build();
    }
}
