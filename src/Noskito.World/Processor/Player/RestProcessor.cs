using System.Linq;
using System.Threading.Tasks;
using Noskito.World.Game.Entities;
using Noskito.World.Packet.Client.Player;
using Noskito.World.Processor.Extension;

namespace Noskito.World.Processor.Player
{
    public class RestProcessor : PacketProcessor<Rest>
    {
        protected override async Task Process(WorldSession session, Rest packet)
        {
            var character = session.Character;

            foreach (var target in packet.Entities)
            {
                var entity = character.Map.GetEntity<LivingEntity>(target.EntityType, target.EntityId);
                if (entity == null)
                {
                    continue;
                }

                entity.IsSitting = !entity.IsSitting;

                await character.Map.BroadcastRest(entity);
            }
        }
    }
}