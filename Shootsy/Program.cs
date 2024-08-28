using Autofac.Extensions.DependencyInjection;
using Shootsy;

namespace WebApplicationStartup
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            IHostBuilder builder = CreateHostBuilder(currentDirectory, args);
            using IHost host = builder.Build();
            host.Run();
        }


        public static IHostBuilder CreateHostBuilder(string currentDirectory, string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                       .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                       .ConfigureWebHostDefaults(webBuilder =>
                       {
                           webBuilder
                             .UseIISIntegration()
                             .UseContentRoot(currentDirectory)
                             .UseStartup<Startup>();
                       });
        }
    }
}