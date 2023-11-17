using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Noskito.Database.Dto;
using Noskito.Database.Repository;
using Noskito.World.Game.Services;

namespace Noskito.World.Game.Entities
{
    public class EntityFactory
    {
        private readonly MonsterDataRepository monsterDataRepository;

        private readonly Dictionary<int, MonsterDataDTO> cachedMonsterDatas = new();
        
        public EntityFactory(MonsterDataRepository monsterDataRepository)
        {
            this.monsterDataRepository = monsterDataRepository;
        }

        public async ValueTask<Monster> CreateMonster(MonsterDTO monster)
        {
            var data = cachedMonsterDatas.GetValueOrDefault(monster.GameId);
            if (data == null)
            {
                data = await monsterDataRepository.GetMonsterData(monster.GameId);
                if (data == null)
                {
                    throw new InvalidOperationException($"Failed to create monster {monster.GameId}");
                }

                cachedMonsterDatas[monster.GameId] = data;
            }

            return new Monster
            {
                Id = monster.Id,
                GameId = monster.GameId,
                Name = data.Name,
                Hp = data.MaxHp,
                Mp = data.MaxMp,
                MaxHp = data.MaxHp,
                MaxMp = data.MaxMp,
                Level = data.Level,
                IsHostile = data.IsHostile,
                Experience = data.Experience,
                JobExperience = data.JobExperience,
                Race = data.Race,
                Element = data.Element,
                ElementPercentage = data.ElementPercentage,
                FireResistance = data.FireResistance,
                WaterResistance = data.WaterResistance,
                LightResistance = data.LightResistance,
                ShadowResistance = data.ShadowResistance,
                SeekRange = data.SeekRange,
                RespawnTime = data.RespawnTime,
                Speed = data.Speed,
                Position = new Position
                {
                    X = monster.X,
                    Y = monster.Y
                }
            };
        }
    }
}