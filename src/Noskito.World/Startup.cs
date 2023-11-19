using DataLoader.Module;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Noskito.Communication.Extension;
using Noskito.Database.Extension;
using Noskito.Logging;
using Noskito.Modules;
using Noskito.Modules.Exceptions;
using Noskito.World.Extension;
using Noskito.World.Packet.Extension;
using Noskito.World.Processor.Extension;
using System.Collections.Generic;

namespace Noskito.World
{
    public class Startup
    {
        private static ServiceProvider GetModules()
        {
            return new ServiceCollection()
                .AddTransient<IDependencyInjectorModule, DataLoaderModule>()
                .BuildServiceProvider();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.UseLoggingModule();
            services.AddPacketFactory();
            services.AddPacketProcessing();
            services.AddDatabase();
            services.AddServerService();
            services.AddNetworkServer();
            services.AddManagers();
            services.AddFactories();
            services.AddGameServices();

            foreach (IDependencyInjectorModule module in GetModules().GetRequiredService<IEnumerable<IDependencyInjectorModule>>())
            {
                try
                {
                    Log.Info($"[MODULE_LOADER] Loading generic module {module.Name}");
                    module.RegisterServices(services);
                }
                catch (ModuleException e)
                {
                    Log.Error($"{module.Name} : module.OnLoad", e);
                }
            }

            services.AddHostedService<WorldService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
        }
    }
}