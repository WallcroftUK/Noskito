using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetty.Common.Utilities;
using Noskito.Common.Logging;
using Noskito.Enum;
using Noskito.World.Game.Entities;
using Noskito.World.Packet.Server;

namespace Noskito.World.Game.Maps
{
    public class Map
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public byte[] Grid { get; init; }
        public int Height { get; init; }
        public int Width { get; init; }

        public IEnumerable<Character> Characters => characters.Values;
        public IEnumerable<Monster> Monsters => monsters.Values;

        private readonly ILogger logger;
        private readonly Dictionary<long, Character> characters = new();
        private readonly Dictionary<long, Monster> monsters = new();

        public Map(ILogger logger)
        {
            this.logger = logger;
        }

        public void AddEntity(Entity entity)
        {
            switch (entity.EntityType)
            {
                case EntityType.Player:
                    characters[entity.Id] = (Character) entity;
                    logger.Information($"Added character: {entity.Id}({entity.Name}) to map {Id}");
                    break;
                case EntityType.Monster:
                    monsters[entity.Id] = (Monster) entity;
                    logger.Information($"Added monster: {entity.Id} to map {Id}");
                    break;
                case EntityType.Npc:
                    break;
                case EntityType.Object:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(entity));
            }
        }

        public Entity GetEntity(EntityType entityType, long id)
        {
            return entityType switch
            {
                EntityType.Monster => monsters.GetValueOrDefault(id),
                EntityType.Player => characters.GetValueOrDefault(id),
                _ => default
            };
        }
        
        public T GetEntity<T>(EntityType entityType, long id) where T : Entity
        {
            var entity = GetEntity(entityType, id);
            if (entity == null)
            {
                return default;
            }

            if (entity is T castedEntity)
            {
                return castedEntity;
            }
            
            return default;
        }
        
        public bool IsWalkable(Position position)
        {
            if (position.X > Width || position.X < 0 || position.Y > Height || position.Y < 0)
            {
                return false;
            }

            var b = Grid[4 + position.Y * Width + position.X];
            
            return b == 0 || b == 2 || (b >= 16 && b <= 19);
        }

        public async Task Broadcast<T>(T packet, Func<WorldSession, bool> predicate = null) where T : SPacket
        {
            foreach (var character in Characters)
            {
                if (predicate != null)
                {
                    var match = predicate.Invoke(character.Session);
                    if (!match)
                    {
                        continue;
                    }
                }
                
                await character.Session.SendPacket(packet);
            }
        }
    }
}