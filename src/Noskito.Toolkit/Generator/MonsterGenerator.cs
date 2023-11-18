using System.Collections.Generic;
using System.IO;
using System.Linq;
using Noskito.Common.Logging;
using Noskito.Toolkit.Objects;
using Noskito.Toolkit.Serialization;
using TextReader = Noskito.Toolkit.Parser.Reader.TextReader;

namespace Noskito.Toolkit.Generator
{
    public class MonsterGenerator : IGenerator
    {
        private readonly ILogger logger;
        private readonly ISerialization serialization;

        public MonsterGenerator(ILogger logger, ISerialization serialization)
        {
            this.logger = logger;
            this.serialization = serialization;
        }

        public void Generate(DirectoryInfo directory)
        {

            var packetDirectory = directory.GetDirectories().FirstOrDefault(x => x.Name == "Packets");
            if (packetDirectory == null)
            {
                logger.Warning("Missing Packets directory, skipping packets parsing");
                return;
            }

            var monstersDirectory = directory.GetDirectories().FirstOrDefault(x => x.Name == "Monsters");
            if (monstersDirectory == null)
            {
                logger.Warning("Missing Monsters directory, skipping monsters parsing");
                return;
            }

            var file = packetDirectory.GetFiles().FirstOrDefault(x => x.Name == "packet.txt");
            if (file == null)
            {
                return;
            }

            var content = TextReader.FromFile(file)
                .SplitLineContent(' ')
                .TrimLines()
                .SkipEmptyLines()
                .GetContent();

            var maps = content.GetRegions("c_map");
            foreach (var map in maps)
            {
                var monsters = new List<Monster>();
                var mapLine = map.GetLine("c_map");

                if (mapLine.GetValue(3) != "1")
                {
                    continue;
                }

                var mapId = short.Parse(mapLine.GetValue(2));

                foreach (var line in map.GetLines("in"))
                {

                    if (line.Length <= 7 || line.GetValue<int>(1) != 3)
                    {
                        continue;
                    }
                    monsters.Add(new Monster
                    {
                        GameId = line.GetValue<int>(2),
                        X = line.GetValue<int>(4),
                        Y = line.GetValue<int>(5)
                    });
                }
            
                using (TextWriter writer = File.CreateText(Path.Combine(monstersDirectory.FullName, $"map_{mapId}.yml")))
                {
                    serialization.Serialize(writer, new MapMonsters
                    {
                        MapId = mapId,
                        Monsters = monsters
                    });
                }
            }
        }
    }
}