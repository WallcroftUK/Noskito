using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Noskito.Common.Logging;
using Noskito.Database.Dto;
using Noskito.Database.Repository;
using Noskito.Toolkit.Objects;
using Noskito.Toolkit.Serialization;

namespace Noskito.Toolkit.Parser
{
    public class MonsterParser : IParser
    {
        private readonly ILogger logger;
        private readonly ISerialization serialization;
        private readonly MonsterRepository monsterRepository;

        public MonsterParser(ILogger logger, ISerialization serialization, MonsterRepository monsterRepository)
        {
            this.logger = logger;
            this.serialization = serialization;
            this.monsterRepository = monsterRepository;
        }

        public async Task Parse(DirectoryInfo directory)
        {
            logger.Information("Parsing monsters");
            var monsterDirectory = directory.GetDirectories().FirstOrDefault(x => x.Name == "Monsters");
            if (monsterDirectory == null)
            {
                logger.Warning("Missing Monsters directory, skipping monster parsing");
                return;
            }

            var monsters = new List<MonsterDTO>();
            foreach (var file in monsterDirectory.EnumerateFiles("*.yml"))
            {
                using (var reader = file.OpenText())
                {
                    var parsed = serialization.Deserialize<MapMonsters>(reader);
                    if (parsed == null)
                    {
                        continue;
                    }

                    foreach (var monster in parsed.Monsters)
                    {
                        monsters.Add(new MonsterDTO
                        {
                            GameId = monster.GameId,
                            X = monster.X,
                            Y = monster.Y,
                            MapId = parsed.MapId
                        });
                    }
                }
            }

            await monsterRepository.SaveAll(monsters);
            
            logger.Information($"Saved {monsters.Count} monsters");
        }
    }
}