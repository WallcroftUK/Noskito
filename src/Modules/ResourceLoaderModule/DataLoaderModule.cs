using DataLoader.Module.Services;
using Microsoft.Extensions.DependencyInjection;
using Noskito.Database.Dto.BCards;
using Noskito.Database.Dto.Items;
using Noskito.Database.Extension;
using Noskito.Logging;
using Noskito.Modules;

namespace DataLoader.Module
{
    public class DataLoaderModule : IDependencyInjectorModule
    {
        public string Name => nameof(DataLoaderModule);

        public void RegisterServices(IServiceCollection services)
        {
            Log.Info("test");
            services.AddSingleton(s => new RsrcConfigLoader(Environment.GetEnvironmentVariable("DATA_PATH") ?? "data"));
            services.AddSingleton<RsrcProv<ItemDTO>, ItemRsrcFileLoaderService>();
            services.AddSingleton<RsrcProv<CardDTO>, CardRsrcFileLoaderService>();
        }
    }
}