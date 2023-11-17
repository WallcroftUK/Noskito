using System.Collections.Generic;
using Noskito.Enum;
using Noskito.World.Packet.Extension;

namespace Noskito.World.Packet.Client.Player
{
    public class Rest : CPacket
    {
        public IEnumerable<RestEntity> Entities { get; init; }
    }

    public class RestEntity
    {
        public EntityType EntityType { get; init; }
        public long EntityId { get; init; }
    }
    
    public class RestCreator : CPacketCreator<Rest>
    {
        protected override Rest CreatePacket(string[] parameters)
        {
            var count = parameters[0].ToInt();
            var entities = new List<RestEntity>();
            for (var i = 0; i < count; i += 2)
            {
                entities.Add(new RestEntity
                {
                    EntityType = parameters[i + 1].ToEnum<EntityType>(),
                    EntityId = parameters[i + 2].ToLong()
                });
            }

            return new Rest
            {
                Entities = entities
            };
        }
    }
}