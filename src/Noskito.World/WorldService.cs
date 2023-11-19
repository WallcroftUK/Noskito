using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Noskito.Communication.Server;
using Noskito.Logging;
using Noskito.World.Network;

namespace Noskito.World
{
    public class WorldService : IHostedService
    {
        private readonly NetworkServer _server;
        private readonly ServerService _serverService;

        public WorldService(NetworkServer server, ServerService serverService)
        {
            _server = server;
            _serverService = serverService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            bool clusterOnline;
            do
            {
                clusterOnline = await _serverService.IsClusterOnline();
            } while (!clusterOnline);

            var worldServer = new WorldServer
            {
                Host = "127.0.0.1",
                Port = 20000,
                Name = "Noskito"
            };

            if (await _serverService.AddWorldServer(worldServer))
            {
                Log.Info("Starting server");
                await _server.Start(worldServer.Port);
            }
            else
            {
                Log.Warn("Failed to register world server");
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            Log.Info("Stopping server");
            await _server.Stop();
        }
    }
}
