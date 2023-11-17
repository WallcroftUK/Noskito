using Noskito.Enum;
using Noskito.World.Packet.Extension;

namespace Noskito.World.Packet.Server.Entities
{
    public class In : SPacket
    {
        public EntityType EntityType { get; init; }
        public string Name { get; init; }
        public int GameId { get; init; }
        public long EntityId { get; init; }
        public int X { get; init; }
        public int Y { get; init; }
        public Direction Direction { get; init; }
        
        public InMonster Monster { get; init; }
        public InCharacter Character { get; init; }
    }

    public class InMonster
    {
        public int Hp { get; init; }
        public int Mp { get; init; }
        public bool IsSitting { get; init; }
        public SpawnEffect SpawnEffect { get; init; }
    }

    public class InCharacter
    {
        public AuthorityType Authority { get; init; }
        public Gender Gender { get; init; }
        public HairStyle HairStyle { get; init; }
        public HairColor HairColor { get; init; }
        public Job Job { get; init; }
        public int Hp { get; init; }
        public int Mp { get; init; }
        public bool IsSitting { get; init; }
        public int Level { get; init; }
        public int HeroLevel { get; init; }
        public bool Invisible { get; init; }
        public ReputationIcon ReputationIcon { get; init; }
        public int Size { get; init; }
    }

    public class InCreator : SPacketCreator<In>
    {
        protected override string CreatePacket(In source)
        {
            var output = $"in {source.EntityType.Format()} ";
            if (source.EntityType == EntityType.Player)
            {
                output += $"{source.Name} - {source.EntityId} ";
            }
            else
            {
                output += $"{source.GameId} {source.EntityId} ";
            }

            output += $"{source.X} " +
                      $"{source.Y} ";

            if (source.EntityType != EntityType.Object)
            {
                output += $"{source.Direction.Format()} ";
            }

            if (source.EntityType == EntityType.Player)
            {
                var character = source.Character;

                output += $"{character.Authority.Format()} " +
                          $"{character.Gender.Format()} " +
                          $"{character.HairColor.Format()} " +
                          $"{character.HairStyle.Format()} " +
                          $"{character.Job.Format()} " +
                          $"-1.-1.-1.-1.-1.-1.-1.-1.-1.-1 " + // TODO : Equipments
                          $"{character.Hp} " +
                          $"{character.Mp} " +
                          $"{character.IsSitting.Format()} " +
                          $"-1 " +
                          $"0 " +
                          $"0 " +
                          $"0 " +
                          $"0 " +
                          $"0 " +
                          $"0 " +
                          $"0 " +
                          $"0 " +
                          $"-1 " +
                          $"- " +
                          $"{character.ReputationIcon.Format()} " +
                          $"{character.Invisible.Format()} " +
                          $"0 " +
                          $"0 " +
                          $"0 " +
                          $"{character.Level} " +
                          $"0 " +
                          $"0|0|0 " +
                          $"0 " +
                          $"0 " +
                          $"{character.Size} " +
                          $"{character.HeroLevel} " +
                          $"0";
            }
            else if (source.EntityType == EntityType.Monster || source.EntityType == EntityType.Npc)
            {
                var monster = source.Monster;

                output += $"{monster.Hp} " +
                          $"{monster.Mp} " +
                          $"0 " +
                          $"0 " +
                          $"0 " +
                          $"-1 " +
                          $"{monster.SpawnEffect.Format()} " +
                          $"{monster.IsSitting.Format()} " +
                          $"-1 " +
                          $"- " +
                          $"0 " +
                          $"0 " +
                          $"0 " +
                          $"0 " +
                          $"0 " +
                          $"0 " +
                          $"0 " +
                          $"0 " +
                          $"0 " +
                          $"0 " +
                          $"0 ";
            }

            return output;
        }
    }
}