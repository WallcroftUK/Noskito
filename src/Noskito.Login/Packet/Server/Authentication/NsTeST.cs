using System.Collections.Generic;

namespace Noskito.Login.Packet.Server.Authentication
{
    public class NsTeST : SPacket
    {
        public byte RegionId { get; init; }
        public string Account { get; init; }
        public int EncryptionKey { get; init; }
        public List<Server> Servers { get; init; } = new();

        public class Server
        {
            public string Host { get; init; }
            public int Port { get; init; }
            public int Color { get; init; }
            public int Count { get; init; }
            public int Id { get; init; }
            public string Name { get; init; }
        }
    }

    public class NsTeSTCreator : SPacketCreator<NsTeST>
    {
        protected override string CreatePacket(NsTeST source)
        {
            string lastGroup = string.Empty;
            int worldGroupCount = 0;

            var packet = $"NsTeST {source.RegionId} {source.Account} -99 0 -99 0 -99 0 -99 0 -99 0 -99 0 -99 0 -99 0 -99 0 -99 0 -99 0 -99 0 -99 0 -99 0 -99 0 -99 0 -99 0 -99 0 -99 0 -99 0 -99 0 -99 0 -99 0 -99 0 -99 0 -99 0 -99 0 -99 0 -99 0 -99 0 -99 0 -99 0 -99 0 -99 0 -99 0 -99 0 {source.EncryptionKey} ";
            foreach (var server in source.Servers )
            {
                if (lastGroup != server.Name)
                {
                    worldGroupCount++;
                }

                packet += $"{server.Host}:{server.Port}:{server.Color}:{server.Id}.{worldGroupCount}.{server.Name} "; 
                //{server.Count} instead of {worldGroupCount} need look why it shows channel 0
            }
            //yea i know it looks bad but :D 
            packet += "-1:-1:-1:10000.10000.1";

            return packet;
        }
    }
}