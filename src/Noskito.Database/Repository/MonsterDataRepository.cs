using System.Collections.Generic;
using System.Threading.Tasks;
using Noskito.Database.Dto;
using Noskito.Database.Entity;

namespace Noskito.Database.Repository
{
    public class MonsterDataRepository
    {
        private readonly DbContextFactory contextFactory;
        private readonly Mapper<DbMonsterData, MonsterDataDTO> mapper;

        public MonsterDataRepository(DbContextFactory contextFactory, Mapper<DbMonsterData, MonsterDataDTO> mapper)
        {
            this.contextFactory = contextFactory;
            this.mapper = mapper;
        }

        public async Task<MonsterDataDTO> GetMonsterData(int gameId)
        {
            using (var context = contextFactory.CreateContext())
            {
                var entity = await context.MonstersData.FindAsync(gameId);
                if (entity == null)
                {
                    return default;
                }

                return mapper.Map(entity);
            }
        }

        public async Task SaveAll(IEnumerable<MonsterDataDTO> values)
        {
            using (var context = contextFactory.CreateContext())
            {
                await context.MonstersData.AddRangeAsync(mapper.Map(values));
                await context.SaveChangesAsync();
            }
        }
    }
}