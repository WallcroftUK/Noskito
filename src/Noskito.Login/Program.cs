using System.Linq;
using System.Reflection;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Noskito.Login
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            ObscuredHeader();
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureLogging(x =>
                {
                    x.ClearProviders();
                    x.AddFilter("Microsoft", LogLevel.Warning);
                })
                .ConfigureWebHostDefaults(x =>
                {
                    x.UseKestrel(s => { s.ListenAnyIP(14000, options => options.Protocols = HttpProtocols.Http2); });
                    x.UseStartup<Startup>();
                })
                .UseConsoleLifetime()
                .Build();

            using (host)
            {
                await host.StartAsync();
                await host.WaitForShutdownAsync();
            }
        }
        private static void ObscuredHeader()
        {
            Console.Title = "Noskito - Login";
            const string text = @"
███╗   ██╗ ██████╗ ███████╗██╗  ██╗██╗████████╗ ██████╗     ██╗      ██████╗  ██████╗ ██╗███╗   ██╗
████╗  ██║██╔═══██╗██╔════╝██║ ██╔╝██║╚══██╔══╝██╔═══██╗    ██║     ██╔═══██╗██╔════╝ ██║████╗  ██║
██╔██╗ ██║██║   ██║███████╗█████╔╝ ██║   ██║   ██║   ██║    ██║     ██║   ██║██║  ███╗██║██╔██╗ ██║
██║╚██╗██║██║   ██║╚════██║██╔═██╗ ██║   ██║   ██║   ██║    ██║     ██║   ██║██║   ██║██║██║╚██╗██║
██║ ╚████║╚██████╔╝███████║██║  ██╗██║   ██║   ╚██████╔╝    ███████╗╚██████╔╝╚██████╔╝██║██║ ╚████║
╚═╝  ╚═══╝ ╚═════╝ ╚══════╝╚═╝  ╚═╝╚═╝   ╚═╝    ╚═════╝     ╚══════╝ ╚═════╝  ╚═════╝ ╚═╝╚═╝  ╚═══╝
";
            string separator = new string('=', Console.WindowWidth);
            string logo = text.Split('\n')
                .Select(s => string.Format("{0," + (Console.WindowWidth / 2 + s.Length / 2) + "}\n", s))
                .Aggregate("", (current, i) => current + i);

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(separator + logo + $"Version: {Assembly.GetExecutingAssembly().GetName().Version}\n" + separator);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}