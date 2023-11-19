using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Noskito.Database.Dto;
using Noskito.Database.Repository;
using Noskito.Enum;
using Noskito.Logging;
using Noskito.Toolkit.Parser.Reader;
using TextReader = Noskito.Toolkit.Parser.Reader.TextReader;

namespace Noskito.Toolkit.Parser
{
    public class MonsterDataParser : IParser
    {
        private readonly MonsterDataRepository monsterDataRepository;

        private static readonly int[] Hp = CreateHpArray();
        private static readonly int[] Mp = CreateMpArray();
        private static readonly int[] Experience = CreateExperienceArray();
        private static readonly int[] JobExperience = CreateJobExperienceArray();
        
        public MonsterDataParser(MonsterDataRepository monsterDataRepository)
        {
            this.monsterDataRepository = monsterDataRepository;
        }

        public async Task Parse(DirectoryInfo directory)
        {
            Log.Info("Parsing monsters data");

            var datDirectory = directory.GetDirectories().FirstOrDefault(x => x.Name == "Data");
            if (datDirectory == null)
            {
                Log.Warn("Missing Data directory, skipping dat parsing");
                return;
            }

            var langDirectory = directory.GetDirectories().FirstOrDefault(x => x.Name == "Lang");
            if (datDirectory == null)
            {
                Log.Warn("Missing Data directory, skipping dat parsing");
                return;
            }

            var datFile = datDirectory.GetFiles().FirstOrDefault(x => x.Name == "monster.dat");
            if (datFile == null)
            {
                Log.Warn("Can't found monster.dat, skipping monster parsing");
                return;
            }

            var langFile = langDirectory.GetFiles().FirstOrDefault(x => x.Name == "_code_uk_monster.txt");
            if (langFile == null)
            {
                Log.Warn("Can't found _code_uk_monster.txt, skipping monster parsing");
                return;
            }

            var dictionary = new Dictionary<string, string>();

            TextContent npcIdLangContent = TextReader.FromFile(langFile)
                .SkipCommentedLines("#")
                .SkipEmptyLines()
                .SplitLineContent('\t')
                .TrimLines()
                .GetContent();
            foreach (var kvp in dictionary)
            {
                Console.WriteLine($"{kvp.Key}: {kvp.Value}");
            }
            foreach (var line in npcIdLangContent.GetLines("zts"))
            {
                var key = line.GetValue(0);
                var value = line.GetValue(1);

                if (!dictionary.ContainsKey(key))
                {
                    dictionary.Add(key, value);
                }
            }

            TextContent monsterContent = TextReader.FromFile(datFile)
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

                // Extract name using dictionaryIdLang
                var name = dictionary.ContainsKey(nameKey) ? dictionary[nameKey] : "";

                datas.Add(new MonsterDataDTO
                {
                    Id = id,
                    Name = name,
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

            Log.Info($"Saved {datas.Count} monsters data");
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