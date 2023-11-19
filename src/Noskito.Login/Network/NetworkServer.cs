using System.Threading.Tasks;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Noskito.Logging;
using Noskito.Login.Network.Pipeline;
using Noskito.Login.Packet;
using Noskito.Login.Processor;

namespace Noskito.Login.Network
{
    public sealed class NetworkServer
    {
        private readonly ServerBootstrap bootstrap;
        private readonly MultithreadEventLoopGroup bossGroup, workerGroup;

        private IChannel channel;

        public NetworkServer(PacketFactory packetFactory, ProcessorManager processorManager)
        {
            bossGroup = new MultithreadEventLoopGroup(1);
            workerGroup = new MultithreadEventLoopGroup();

            bootstrap = new ServerBootstrap()
                .Option(ChannelOption.SoBacklog, 100)
                .Group(bossGroup, workerGroup)
                .Channel<TcpServerSocketChannel>()
                .ChildHandler(new ActionChannelInitializer<IChannel>(x =>
                {
                    var pipeline = x.Pipeline;

                    var client = new NetworkClient(x);
                    var session = new LoginSession(client);

                    client.PacketReceived += packet =>
                    {
                        var processor = processorManager.GetPacketProcessor(packet.GetType());
                        if (processor is null) return Task.CompletedTask;

                        return processor.ProcessPacket(session, packet);
                    };

                    pipeline.AddLast("decoder", new Decoder());
                    pipeline.AddLast("deserializer", new Deserializer(packetFactory));
                    pipeline.AddLast("client", client);
                    pipeline.AddLast("encoder", new Encoder());
                    pipeline.AddLast("serializer", new Serializer(packetFactory));
                }));
        }

        public async Task Start(int port)
        {
            channel = await bootstrap.BindAsync(port);

            Log.Debug($"Server successfully started on port {port}");
        }

        public async Task Stop()
        {
            await channel.CloseAsync();

            await bossGroup.ShutdownGracefullyAsync();
            await workerGroup.ShutdownGracefullyAsync();

            Log.Debug("Server successfully shutdown");
        }
    }
}