using Noskito.Database.Dto;
using Noskito.Enum;

namespace Noskito.World.Game.Entities
{
    public class Monster : LivingEntity
    {
        public int GameId { get; init; }
        public bool IsHostile { get; init; }
        
        public int Experience { get; init; }
        public int JobExperience { get; init; }

        public int SeekRange { get; init; }
        public int RespawnTime { get; init; }
        
        public Monster()
        {
            EntityType = EntityType.Monster;
        }
    }
}