using System.IO;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Hosting;

namespace Arbor.KVConfiguration.Samples.AspNetCore
{
    public static class Program
    {
        [PublicAPI]
        public static void Main(string[] args)
        {
            IWebHost host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
