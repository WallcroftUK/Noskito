
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Noskito.Database.Dto;
using Noskito.Database.Repository;
using Noskito.Logging;
using TextReader = Noskito.Toolkit.Parser.Reader.TextReader;

namespace Noskito.Toolkit.Parser
{
    public class MapParser : IParser
    {
        private readonly MapRepository mapRepository;

        public MapParser(MapRepository mapRepository)
        {
            this.mapRepository = mapRepository;
        }

        public async Task Parse(DirectoryInfo directory)
        {
            Log.Info("Parsing maps");
            
            var mapDirectory = directory.GetDirectories().FirstOrDefault(x => x.Name == "Maps");
            if (mapDirectory == null)
            {
                Log.Warn("Missing Maps directory, skipping map parsing");
                return;
            }

            var datDirectory = directory.GetDirectories().FirstOrDefault(x => x.Name == "Data");
            if (datDirectory == null)
            {
                Log.Warn("Missing Data directory, skipping dat parsing");
                return;
            }

            var mapFiles = mapDirectory.EnumerateFiles("*");
            if (!mapFiles.Any())
            {
                Log.Warn("Can't found any map file in Maps directory, skipping map parsing");
                return;
            }

            var mapIdData = datDirectory.GetFiles().FirstOrDefault(x => x.Name == "MapIDData.dat");
            if (mapIdData == null)
            {
                Log.Warn("Can't found MapIDData.dat, skipping map parsing");
                return;
            }
            
            var content = Reader.TextReader.FromFile(mapIdData)
                .SkipLines(x => x.StartsWith("DATA"))
                .SkipCommentedLines("#")
                .SkipEmptyLines()
                .SplitLineContent(' ')
                .TrimLines()
                .GetContent();

            var mapNameKeys = new Dictionary<int, string>();
            foreach (var line in content.Lines)
            {
                var firstMapId = line.GetValue<int>(0);
                var secondMapId = line.GetValue<int>(1);
                var nameKey = line.GetValue(4);

                for (var i = firstMapId; i <= secondMapId; i++)
                {
                    mapNameKeys[i] = nameKey;
                }
            }
            
            var maps = new List<MapDTO>();
            foreach (var mapFile in mapFiles)
            {
                var mapId = int.Parse(Path.GetFileNameWithoutExtension(mapFile.Name));
                maps.Add(new MapDTO
                {
                    Id = int.Parse(Path.GetFileNameWithoutExtension(mapFile.Name)),
                    Name = mapNameKeys.GetValueOrDefault(mapId, "UNDEFINED"),
                    Grid = await File.ReadAllBytesAsync(mapFile.FullName)
                });
            }

            await mapRepository.SaveAll(maps);

            Log.Info($"Saved {maps.Count} maps");
        }
    }
}