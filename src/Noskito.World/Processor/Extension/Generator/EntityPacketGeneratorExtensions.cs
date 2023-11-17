using Noskito.World.Game.Entities;
using Noskito.World.Packet.Server.Entities;

namespace Noskito.World.Processor.Extension.Generator
{
    public static class EntityPacketGeneratorExtensions
    {
        public static Rest CreateRestPacket(this LivingEntity entity)
        {
            return new Rest
            {
                EntityType = entity.EntityType,
                EntityId = entity.Id,
                IsSitting = entity.IsSitting
            };
        }

        public static Mv CreateMvPacket(this LivingEntity entity)
        {
            return new Mv
            {
                EntityType = entity.EntityType,
                EntityId = entity.Id,
                X = entity.Position.X,
                Y = entity.Position.Y,
                Speed = entity.Speed
            };
        }
    }
}