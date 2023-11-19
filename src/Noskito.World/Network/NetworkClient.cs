using System;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using Noskito.Logging;
using Noskito.World.Packet.Client;
using Noskito.World.Packet.Server;

namespace Noskito.World.Network
{
    public class NetworkClient : ChannelHandlerAdapter
    {
        private readonly IChannel channel;

        public NetworkClient(IChannel channel)
        {
            this.channel = channel;
        }

        public int EncryptionKey { get; set; }

        public event Func<CPacket, Task> PacketReceived;
        public event Func<Task> Disconnected; 

        public Task SendPacket<T>(T packet) where T : SPacket
        {
            return channel.WriteAndFlushAsync(packet);
        }

        public Task Disconnect()
        {
            return channel.DisconnectAsync();
        }

        public override async Task DisconnectAsync(IChannelHandlerContext context)
        {
            if (Disconnected != null)
            {
                await Disconnected?.Invoke();
            }
        }

        public override async void ChannelRead(IChannelHandlerContext context, object message)
        {
            if (message is not CPacket packet)
            {
                Log.Debug("Received a non ClientPacket message");
                return;
            }

            try
            {
                if (PacketReceived != null) await PacketReceived?.Invoke(packet);
            }
            catch (Exception e)
            {
                Log.Error($"Something happened when processing packet {packet.GetType().Name}", e);
            }
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Log.Error($"Something happened with client {channel.Id.AsShortText()}", exception);
        }
    }
}