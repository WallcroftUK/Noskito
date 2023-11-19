using System.Linq;
using System.Threading.Tasks;
using Noskito.Cluster.Manager;
using Noskito.Communication.Server;
using Noskito.Communication.Rpc.Common.Request;
using Noskito.Communication.Rpc.Common.Response;
using Noskito.Communication.Rpc.Server;
using Noskito.Communication.Rpc.Server.Object;
using Noskito.Communication.Rpc.Server.Request;
using Noskito.Communication.Rpc.Server.Response;
using Noskito.Logging;

namespace Noskito.Cluster.Service
{
    public class RpcServerService : IRpcServerService
    {
        private readonly ServerManager serverManager;

        public RpcServerService(ServerManager serverManager)
        {
            this.serverManager = serverManager;
        }

        public ValueTask<BoolResponse> IsClusterOnline(EmptyRequest request)
        {
            return ValueTask.FromResult(new BoolResponse
            {
                Value = true
            });
        }

        public ValueTask<BoolResponse> IsMaintenanceMode(EmptyRequest request)
        {
            return ValueTask.FromResult(new BoolResponse
            {
                Value = serverManager.IsMaintenanceMode
            });
        }

        public ValueTask<GetWorldServersResponse> GetWorldServers(EmptyRequest request)
        {
            return ValueTask.FromResult(new GetWorldServersResponse
            {
                Servers = serverManager.GetWorldServers().Select(x => new WorldServerObject
                {
                    Host = x.Host,
                    Port = x.Port,
                    Name = x.Name
                })
            });
        }

        public ValueTask<BoolResponse> AddWorldServer(AddWorldServerRequest request)
        {
            var server = request.Server;
            var added = serverManager.AddWorldServer(new WorldServer
            {
                Host = server.Host,
                Port = server.Port,
                Name = server.Name
            });

            Log.Info($"World {server.Name} with Ip {server.Host} and Port {server.Port} Registered");

            return ValueTask.FromResult(new BoolResponse
            {
                Value = added
            });
        }
    }
}