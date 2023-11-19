using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Noskito.Modules;
using System.Linq;
using System.Reflection;
using Noskito.Logging;


namespace Noskito.World
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            ObscuredHeader();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            using IHost httpServer = ConfigureCustomHostBuilder(args).Build();
            {
                IServiceProvider services = httpServer.Services;

                IEnumerable<IServerModule> modules = services.GetServices<IServerModule>();
                foreach (IServerModule module in modules)
                {
                    module.OnLoad();
                }

                await httpServer.StartAsync();

                await httpServer.WaitForShutdownAsync();

                Log.Warn("Properly shutting down server...");

                await httpServer?.StopAsync();
            }
        }
        private static IHostBuilder ConfigureCustomHostBuilder(string[] args)
        {
            var hostBuilder = Host.CreateDefaultBuilder(args);

            hostBuilder.ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseKestrel();
                //webBuilder.UseKestrel(s => { s.ListenAnyIP(16000, options => options.Protocols = HttpProtocols.Http2); });
                webBuilder.UseStartup<Startup>();
            });

            return hostBuilder;
        }
        private static void ObscuredHeader()
        {
            Console.Title = "Noskito - World";
            const string text = @"
███╗   ██╗ ██████╗ ███████╗██╗  ██╗██╗████████╗ ██████╗     ██╗    ██╗ ██████╗ ██████╗ ██╗     ██████╗ 
████╗  ██║██╔═══██╗██╔════╝██║ ██╔╝██║╚══██╔══╝██╔═══██╗    ██║    ██║██╔═══██╗██╔══██╗██║     ██╔══██╗
██╔██╗ ██║██║   ██║███████╗█████╔╝ ██║   ██║   ██║   ██║    ██║ █╗ ██║██║   ██║██████╔╝██║     ██║  ██║
██║╚██╗██║██║   ██║╚════██║██╔═██╗ ██║   ██║   ██║   ██║    ██║███╗██║██║   ██║██╔══██╗██║     ██║  ██║
██║ ╚████║╚██████╔╝███████║██║  ██╗██║   ██║   ╚██████╔╝    ╚███╔███╔╝╚██████╔╝██║  ██║███████╗██████╔╝
╚═╝  ╚═══╝ ╚═════╝ ╚══════╝╚═╝  ╚═╝╚═╝   ╚═╝    ╚═════╝      ╚══╝╚══╝  ╚═════╝ ╚═╝  ╚═╝╚══════╝╚═════╝ 
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