using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Noskito.Database.Entity
{
    public class DbMonster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; init; }
        
        [Required]
        public int GameId { get; init; }
        
        [Required]
        public int MapId { get; init; }
        
        [Required]
        public int X { get; init; }
        
        [Required]
        public int Y { get; init; }
        
        public virtual DbMap Map { get; init; }
        public virtual DbMonsterData Data { get; init; }
    }
}