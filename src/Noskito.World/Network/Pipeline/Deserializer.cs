using System.Collections.Generic;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Noskito.Logging;
using Noskito.World.Packet;

namespace Noskito.World.Network.Pipeline
{
    public class Deserializer : MessageToMessageDecoder<string>
    {
        private readonly PacketFactory packetFactory;

        public Deserializer(PacketFactory packetFactory)
        {
            this.packetFactory = packetFactory;
        }

        protected override void Decode(IChannelHandlerContext context, string message, List<object> output)
        {
            if (string.IsNullOrEmpty(message)) return;

            var packet = packetFactory.CreatePacket(message);
            if (packet is null)
            {
                Log.Debug("Failed to create typed packet, skipping it");
                return;
            }

            output.Add(packet);

            Log.Debug($"In [{packet.GetType().Name}]: {message}");
        }
    }
}