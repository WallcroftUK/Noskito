using Microsoft.Extensions.DependencyInjection;
using Noskito.World.Game.Entities;
using Noskito.World.Game.Maps;
using Noskito.World.Game.Services;
using Noskito.World.Network;

namespace Noskito.World.Extension
{
    public static class ServiceCollectionExtensions
    {
        public static void AddNetworkServer(this IServiceCollection services)
        {
            services.AddTransient<NetworkServer>();
        }

        public static void AddManagers(this IServiceCollection services)
        {
            services.AddSingleton<MapManager>();
        }

        public static void AddFactories(this IServiceCollection services)
        {
            services.AddTransient<EntityFactory>();
            services.AddTransient<MapFactory>();
        }

        public static void AddGameServices(this IServiceCollection services)
        {
            services.AddTransient<ReputationService>();
        }
    }
}