using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Noskito.Database.Dto;
using Noskito.Database.Entity;

namespace Noskito.Database.Repository
{
    public class MonsterRepository
    {
        private readonly DbContextFactory contextFactory;
        private readonly Mapper<DbMonster, MonsterDTO> mapper;

        public MonsterRepository(DbContextFactory contextFactory, Mapper<DbMonster, MonsterDTO> mapper)
        {
            this.contextFactory = contextFactory;
            this.mapper = mapper;
        }

        public async Task<MonsterDTO> GetMonster(long monsterId)
        {
            using (var context = contextFactory.CreateContext())
            {
                var entity = await context.Monsters.FindAsync(monsterId);
                if (entity == null)
                {
                    return default;
                }

                return mapper.Map(entity);
            }
        }

        public async Task<IEnumerable<MonsterDTO>> GetMonstersInMap(int mapId)
        {
            using (var context = contextFactory.CreateContext())
            {
                var entities = await context.Monsters.Where(x => x.MapId == mapId).ToListAsync();
                if (entities == null)
                {
                    return default;
                }

                return mapper.Map(entities);
            }
        }

        public async Task SaveAll(IEnumerable<MonsterDTO> monsters)
        {
            using (var context = contextFactory.CreateContext())
            {
                await context.Monsters.AddRangeAsync(mapper.Map(monsters));
                await context.SaveChangesAsync();
            }
        }
    }
}