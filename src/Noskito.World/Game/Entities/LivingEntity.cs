using Noskito.Enum;

namespace Noskito.World.Game.Entities
{
    public abstract class LivingEntity : Entity
    {
        public int Hp { get; set; }
        public int Mp { get; set; }
        public int MaxHp { get; set; }
        public int MaxMp { get; set; }
        
        public int Level { get; set; }

        public int Speed { get; set; }
        
        public bool IsSitting { get; set; }
        public Direction Direction { get; set; }
        
        public Race Race { get; set; }
        
        public Element Element { get; set; }
        public int ElementPercentage { get; set; }
        
        public int FireResistance { get; set; }
        public int WaterResistance { get; set; }
        public int LightResistance { get; set; }
        public int ShadowResistance { get; set; }
    }
}