using Noskito.Enum;
using Noskito.World.Packet.Extension;

namespace Noskito.World.Packet.Server.Entities
{
    public class Rest : SPacket
    {
        public EntityType EntityType { get; init; }
        public long EntityId { get; init; }
        public bool IsSitting { get; init; }
    }
    
    public class RestCreator : SPacketCreator<Rest>
    {
        protected override string CreatePacket(Rest source)
        {
            return $"rest " +
                   $"{source.EntityType.Format()} " +
                   $"{source.EntityId} " +
                   $"{source.IsSitting.Format()}";
        }
    }
}