using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Noskito.Enum;

namespace Noskito.Database.Entity
{
    public class DbMonsterData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; init; }
        
        [Required]
        public string Name { get; init; }
        
        [Required]
        public int Level { get; init; }
        
        [Required]
        public int MaxHp { get; init; }
        
        [Required]
        public int MaxMp { get; init; }
        
        [Required]
        public Race Race { get; set; }

        [Required]
        public bool IsHostile { get; init; }
        
        [Required]
        public int RespawnTime { get; init; }
        
        [Required]
        public int Speed { get; init; }
        
        [Required]
        public int SeekRange { get; init; }
        
        [Required]
        public int Experience { get; init; }
        
        [Required]
        public int JobExperience { get; init; }
        
        [Required]
        public Element Element { get; init; }
        
        [Required]
        public int ElementPercentage { get; init; }
        
        [Required]
        public int FireResistance { get; init; }
        
        [Required]
        public int WaterResistance { get; init; }
        
        [Required]
        public int LightResistance { get; init; }
        
        [Required]
        public int ShadowResistance { get; init; }

        public ICollection<DbMonster> Monsters { get; init; }
    }
}