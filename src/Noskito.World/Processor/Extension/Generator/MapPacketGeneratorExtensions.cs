using Noskito.World.Game.Maps;
using Noskito.World.Packet.Server.Maps;

namespace Noskito.World.Processor.Extension.Generator
{
    public static class MapPacketGeneratorExtensions
    {
        public static CMap CreateCMapPacket(this Map map, bool joining)
        {
            return new()
            {
                MapId = map.Id,
                IsJoining = joining
            };
        }
        
        
    }
}