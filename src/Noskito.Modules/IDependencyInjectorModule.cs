using Microsoft.Extensions.DependencyInjection;

namespace Noskito.Modules
{
    public interface IDependencyInjectorModule : IModule
    {
        void RegisterServices(IServiceCollection services);
    }
}