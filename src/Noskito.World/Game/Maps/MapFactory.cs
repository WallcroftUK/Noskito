using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Noskito.Common.Extension;
using Noskito.Database.Dto;
using Noskito.Database.Repository;
using Noskito.World.Game.Entities;

namespace Noskito.World.Game.Maps
{
    public class MapFactory
    {
        private readonly MapRepository mapRepository;
        private readonly MonsterRepository monsterRepository;
        private readonly EntityFactory entityFactory;

        private readonly Dictionary<int, MapDTO> cachedMaps = new();

        public MapFactory(MapRepository mapRepository, MonsterRepository monsterRepository, EntityFactory entityFactory)
        {
            this.mapRepository = mapRepository;
            this.monsterRepository = monsterRepository;
            this.entityFactory = entityFactory;
        }

        public async ValueTask<Map> CreateMap(int mapId)
        {
            var map = cachedMaps.GetValueOrDefault(mapId);
            if (map == null)
            {
                map = await mapRepository.Find(mapId);
                if (map == null)
                {
                    throw new InvalidOperationException($"Failed to create map {mapId}");
                }

                cachedMaps[mapId] = map;
            }

            var createdMap = new Map()
            {
                Id = map.Id,
                Grid = map.Grid,
                Width = BitConverter.ToInt16(map.Grid.Slice(0, 2)),
                Height = BitConverter.ToInt16(map.Grid.Slice(2, 2))
            };

            var monsters = await monsterRepository.GetMonstersInMap(mapId);
            if (monsters == null)
            {
                return createdMap;
            }

            foreach (var monster in monsters)
            {
                var createdMonster = await entityFactory.CreateMonster(monster);
                if (createdMonster == null)
                {
                    continue;
                }

                createdMonster.Map = createdMap;
                createdMonster.Map.AddEntity(createdMonster);
            }

            return createdMap;
        }
    }
}