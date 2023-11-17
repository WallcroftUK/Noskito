using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Noskito.Common.Logging;
using Noskito.Database.Dto;
using Noskito.Database.Repository;
using Noskito.Enum;
using Noskito.Toolkit.Parser.Reader;
using TextReader = Noskito.Toolkit.Parser.Reader.TextReader;

namespace Noskito.Toolkit.Parser
{
    public class MonsterDataParser : IParser
    {
        private readonly ILogger logger;
        private readonly MonsterDataRepository monsterDataRepository;

        private static readonly int[] Hp = CreateHpArray();
        private static readonly int[] Mp = CreateMpArray();
        private static readonly int[] Experience = CreateExperienceArray();
        private static readonly int[] JobExperience = CreateJobExperienceArray();
        
        public MonsterDataParser(ILogger logger, MonsterDataRepository monsterDataRepository)
        {
            this.logger = logger;
            this.monsterDataRepository = monsterDataRepository;
        }

        public async Task Parse(DirectoryInfo directory)
        {
            logger.Information("Parsing monsters data");
            
            var file = directory.GetFiles().FirstOrDefault(x => x.Name == "monster.dat");
            if (file == null)
            {
                logger.Warning("Can't found MapIDData.dat, skipping map parsing");
                return;
            }
            
            TextContent monsterContent = TextReader.FromFile(file)
                .SkipCommentedLines("#")
                .SkipEmptyLines()
                .SplitLineContent('\t')
                .TrimLines()
                .GetContent();

            var datas = new List<MonsterDataDTO>();
            var monsterRegions = monsterContent.GetRegions("VNUM");
            foreach (var region in monsterRegions)
            {
                var vnumLine = region.GetLine("VNUM");
                var levelLine = region.GetLine("LEVEL");
                var nameLine = region.GetLine("NAME");
                var hpLine = region.GetLine("HP/MP");
                var raceLine = region.GetLine("RACE");
                var preattLine = region.GetLine("PREATT");
                var expLine = region.GetLine("EXP");
                var attribLine = region.GetLine("ATTRIB");
                
                var id = vnumLine.GetValue<int>(1);
                var level = levelLine.GetValue<int>(1);
                var nameKey = nameLine.GetValue(1);
                var hp = hpLine.GetValue<int>(1);
                var mp = hpLine.GetValue<int>(2);
                var race = raceLine.GetValue<byte>(1);
                var raceType = raceLine.GetValue<byte>(2);
                var hostile = preattLine.GetValue(1) == "1";
                var speed = preattLine.GetValue<int>(3);
                var seek = preattLine.GetValue<int>(4);
                var respawn = preattLine.GetValue<int>(5);
                var experience = expLine.GetValue<int>(1);
                var jobExperience = expLine.GetValue<int>(2);
                var element = attribLine.GetValue<byte>(1);
                var elementPercentage = attribLine.GetValue<int>(2);
                var fireResistance = attribLine.GetValue<int>(3);
                var waterResistance = attribLine.GetValue<int>(4);
                var lightResistance = attribLine.GetValue<int>(5);
                var shadowResistance = attribLine.GetValue<int>(6);

                datas.Add(new MonsterDataDTO
                {
                    Id = id,
                    Name = nameKey,
                    Level = level,
                    MaxHp = hp + Hp[level],
                    MaxMp = mp + Mp[level],
                    Experience = experience + Experience[level],
                    JobExperience = jobExperience + JobExperience[level],
                    Race = GetRace(race, raceType),
                    IsHostile = hostile,
                    SeekRange = seek,
                    Speed = speed,
                    RespawnTime = respawn,
                    Element = (Element)element,
                    ElementPercentage = elementPercentage,
                    FireResistance = fireResistance,
                    WaterResistance = waterResistance,
                    LightResistance = lightResistance,
                    ShadowResistance = shadowResistance
                });
            }

            await monsterDataRepository.SaveAll(datas);
            
            logger.Information($"Saved {datas.Count} monsters data");
        }

        private static Race GetRace(byte race, byte raceType)
        {
            return race switch
            {
                0 => raceType switch
                {
                    0 => Race.LowLevelPlant,
                    1 => Race.LowLevelAnimal,
                    2 => Race.LowLevelMonster,
                    _ => Race.Undefined
                },
                1 => raceType switch
                {
                    0 => Race.HighLevelPlant,
                    1 => Race.HighLevelAnimal,
                    2 => Race.HighLevelMonster,
                    _ => Race.Undefined
                },
                2 => raceType switch
                {
                    0 => Race.Kovolt,
                    1 => Race.Bushi,
                    2 => Race.Catsy,
                    _ => Race.Undefined
                },
                3 => raceType switch
                {
                    0 => Race.Human,
                    2 => Race.Half,
                    3 => Race.Demon,
                    _ => Race.Undefined
                },
                4 => raceType switch
                {
                    0 => Race.Angel,
                    _ => Race.Undefined
                },
                5 => raceType switch
                {
                    0 => Race.LowLevelUndead,
                    2 => Race.HighLevelUndead,
                    _ => Race.Undefined
                },
                6 => raceType switch
                {
                    0 => Race.LowLevelSoul,
                    _ => Race.Undefined
                },
                _ => Race.Undefined
            };
        }

        private static int[] CreateHpArray()
        {
            var array = new int[101];

            var hp = 138;
            var up = 18;
            for(int i = 0; i < 100; i++)
            {
                array[i] = hp;
                up++;
                hp += up;

                if (i == 37)
                {
                    hp = 1765;
                    up = 65;
                }

                if (i < 41)
                {
                    continue;
                }

                if ((99 - i) % 8 == 0)
                {
                    up++;
                }
            }

            return array;
        }

        private static int[] CreateMpArray()
        {
            var array = new int[101];

            array[0] = 10;
            array[1] = 10;
            array[2] = 15;

            var up = 5;
            var stable = true;
            var doubled = false;
            var count = 0;
            for(var i = 3; i < 101; i++)
            {
                if (i % 10 == 1)
                {
                    array[i] += array[i - 1] + up * 2;
                    continue;
                }

                if (!stable)
                {
                    up++;
                    count++;
                    if (count == 2)
                    {
                        if (doubled)
                        {
                            doubled = false;
                        }
                        else
                        {
                            stable = true;
                            doubled = true;
                            count = 0;
                        }
                    }

                    if (count == 4)
                    {
                        stable = true;
                        count = 0;
                    }
                }
                else
                {
                    count++;
                    if (count == 2)
                    {
                        stable = false;
                        count = 0;
                    }
                }

                array[i] = array[i - (i % 10 == 2 ? 2 : 1)] + up;
            }

            return array;
        }

        private static int[] CreateExperienceArray()
        {
            var array = new int[101];
            for (var i = 0; i < 100; i++)
            {
                array[i] = i * 65;
            }

            return array;
        }
        
        private static int[] CreateJobExperienceArray()
        {
            var array = new int[101];
            for (var i = 0; i < 100; i++)
            {
                array[i] = 120;
            }

            return array;
        }
    }
}