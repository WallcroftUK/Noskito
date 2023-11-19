using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProtoBuf.Grpc.Server;
using Noskito.Logging;
using System;
using System.Linq;
using System.Reflection;

namespace Noskito.Cluster
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            ObscuredHeader();
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(x =>
                {
                    x.UseLoggingModule();

                    x.AddGrpc();
                    x.AddCodeFirstGrpc();
                })
                .ConfigureWebHost(x =>
                {
                    x.UseKestrel(s =>
                    {
                        s.ListenLocalhost(15000, options => options.Protocols = HttpProtocols.Http2);
                    });
                    x.UseStartup<Startup>();
                })
                .ConfigureLogging(x =>
                {
                    x.ClearProviders();
                    x.AddFilter("Microsoft", LogLevel.Warning);
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
            Console.Title = "Noskito - Cluster";
            const string text = @"
███╗   ██╗ ██████╗ ███████╗██╗  ██╗██╗████████╗ ██████╗      ██████╗██╗     ██╗   ██╗███████╗████████╗███████╗██████╗ 
████╗  ██║██╔═══██╗██╔════╝██║ ██╔╝██║╚══██╔══╝██╔═══██╗    ██╔════╝██║     ██║   ██║██╔════╝╚══██╔══╝██╔════╝██╔══██╗
██╔██╗ ██║██║   ██║███████╗█████╔╝ ██║   ██║   ██║   ██║    ██║     ██║     ██║   ██║███████╗   ██║   █████╗  ██████╔╝
██║╚██╗██║██║   ██║╚════██║██╔═██╗ ██║   ██║   ██║   ██║    ██║     ██║     ██║   ██║╚════██║   ██║   ██╔══╝  ██╔══██╗
██║ ╚████║╚██████╔╝███████║██║  ██╗██║   ██║   ╚██████╔╝    ╚██████╗███████╗╚██████╔╝███████║   ██║   ███████╗██║  ██║
╚═╝  ╚═══╝ ╚═════╝ ╚══════╝╚═╝  ╚═╝╚═╝   ╚═╝    ╚═════╝      ╚═════╝╚══════╝ ╚═════╝ ╚══════╝   ╚═╝   ╚══════╝╚═╝  ╚═╝
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