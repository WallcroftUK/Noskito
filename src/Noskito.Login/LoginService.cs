using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Noskito.Communication.Server;
using Noskito.Logging;
using Noskito.Login.Network;

namespace Noskito.Login
{
    public class LoginService : IHostedService
    {
        private readonly NetworkServer server;
        private readonly ServerService serverService;

        public LoginService(NetworkServer server, ServerService serverService)
        {
            this.server = server;
            this.serverService = serverService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            bool clusterOnline;
            do
            {
                clusterOnline = await serverService.IsClusterOnline();
            } while (!clusterOnline);

            Log.Info("Starting server");
            await server.Start(4000);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            Log.Info("Stopping server");
            await server.Stop();
        }
    }
}