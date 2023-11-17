using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Noskito.Common.Extension;
using Noskito.Common.Logging;
using Noskito.Database.Dto;
using Noskito.Database.Repository;

namespace Noskito.World.Game.Maps
{
    public class MapManager
    {
        private readonly ILogger logger;
        private readonly MapFactory mapFactory;

        private readonly Dictionary<int, Map> maps = new();
        
        public MapManager(ILogger logger, MapFactory mapFactory)
        {
            this.logger = logger;
            this.mapFactory = mapFactory;
        }

        public async ValueTask<Map> GetMap(int mapId)
        {
            var map = maps.GetValueOrDefault(mapId);
            if (map == null)
            {
                map = await mapFactory.CreateMap(mapId);
                maps[mapId] = map;
            }

            return map;
        }
    }
}