using System.Collections.Generic;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Noskito.Logging;
using Noskito.World.Packet;
using Noskito.World.Packet.Server;

namespace Noskito.World.Network.Pipeline
{
    public class Serializer : MessageToMessageEncoder<SPacket>
    {
        private readonly PacketFactory packetFactory;

        public Serializer(PacketFactory packetFactory)
        {
            this.packetFactory = packetFactory;
        }

        protected override void Encode(IChannelHandlerContext context, SPacket message, List<object> output)
        {
            var packet = packetFactory.CreatePacket(message);
            if (string.IsNullOrEmpty(packet))
            {
                Log.Debug("Empty packet, skipping it");
                return;
            }

            output.Add(packet);

            Log.Debug($"Out [{message.GetType().Name}]: {packet}");
        }
    }
}