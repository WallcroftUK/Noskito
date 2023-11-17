using System.Threading.Tasks;
using Noskito.World.Game.Entities;
using Noskito.World.Game.Maps;
using Noskito.World.Processor.Extension.Generator;

namespace Noskito.World.Processor.Extension
{
    public static class MapPacketExtensions
    {
        public static Task BroadcastIn(this Map map, Monster entity)
        {
            return map.Broadcast(entity.CreateInPacket());
        }

        public static Task BroadcastIn(this Map map, Character character)
        {
            return map.Broadcast(character.CreateInPacket());
        }
        
        public static Task BroadcastRest(this Map map, LivingEntity entity)
        {
            return map.Broadcast(entity.CreateRestPacket());
        }

        public static Task BroadcastMv(this Map map, LivingEntity entity)
        {
            return map.Broadcast(entity.CreateMvPacket());
        }

        public static Task BroadcastMv(this Map map, Character character)
        {
            return map.Broadcast(character.CreateMvPacket(), x => x.Id != character.Session.Id);
        }
    }
}