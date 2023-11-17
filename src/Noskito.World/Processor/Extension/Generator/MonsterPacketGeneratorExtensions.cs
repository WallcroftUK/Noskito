using Noskito.Enum;
using Noskito.World.Game.Entities;
using Noskito.World.Packet.Server.Entities;

namespace Noskito.World.Processor.Extension.Generator
{
    public static class MonsterPacketGeneratorExtensions
    {
        public static In CreateInPacket(this Monster monster)
        {
            return new()
            {
                EntityType = monster.EntityType,
                EntityId = monster.Id,
                GameId = monster.GameId,
                X = monster.Position.X,
                Y = monster.Position.Y,
                Direction = monster.Direction,
                Monster = new InMonster
                {
                    Hp = monster.Hp,
                    Mp = monster.Mp,
                    IsSitting = monster.IsSitting,
                    SpawnEffect = SpawnEffect.None
                }
            };
        }
    }
}