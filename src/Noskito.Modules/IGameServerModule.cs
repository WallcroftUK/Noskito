using Microsoft.Extensions.DependencyInjection;

namespace Noskito.Modules
{
    public interface IGameServerModule : IModule
    {
        void AddDependencies(IServiceCollection services, GameServerInitializer gameServer);
    }
}