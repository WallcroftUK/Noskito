using Noskito.Enum;

namespace Noskito.Database.Dto
{
    public class MonsterDataDTO
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public int Level { get; init; }
        public int MaxHp { get; init; }
        public int MaxMp { get; init; }
        public int Experience { get; init; }
        public int JobExperience { get; init; }
        public Race Race { get; init; }
        public bool IsHostile { get; init; }
        public int RespawnTime { get; init; }
        public int SeekRange { get; init; }
        public int Speed { get; init; }
        public Element Element { get; init; }
        public int ElementPercentage { get; init; }
        public int FireResistance { get; init; }
        public int WaterResistance { get; init; }
        public int LightResistance { get; init; }
        public int ShadowResistance { get; init; }
    }
}