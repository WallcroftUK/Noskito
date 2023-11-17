using System.Linq;
using Noskito.Database.Dto;
using Noskito.Enum;
using Noskito.World.Game.Entities;
using Noskito.World.Packet.Server.CharacterScreen;
using Noskito.World.Packet.Server.Entities;
using Noskito.World.Packet.Server.Maps;
using Noskito.World.Packet.Server.Player;

namespace Noskito.World.Processor.Extension.Generator
{
    public static class CharacterPacketGeneratorExtensions
    {
        public static Stat CreateStatPacket(this Character character)
        {
            return new Stat
            {
                Hp = character.Hp,
                Mp = character.Mp,
                MaxHp = character.MaxHp,
                MaxMp = character.MaxMp,
                Options = 0
            };
        }
        
        public static In CreateInPacket(this Character character)
        {
            return new()
            {
                EntityType = character.EntityType,
                EntityId = character.Id,
                Name = character.Name,
                X = character.Position.X,
                Y = character.Position.Y,
                Direction = character.Direction,
                Character = new InCharacter
                {
                    Authority = character.Authority,
                    Gender = character.Gender,
                    HairStyle = character.HairStyle,
                    HairColor = character.HairColor,
                    Job = character.Job,
                    Hp = character.Hp,
                    Mp = character.Mp,
                    IsSitting = character.IsSitting,
                    Level = character.Level,
                    HeroLevel = character.HeroLevel,
                    Invisible = character.IsInvisible,
                    ReputationIcon = character.ReputationIcon,
                    Size = 10
                }
            };
        }
        
        public static At CreateAtPacket(this Character character)
        {
            return new At
            {
                CharacterId = character.Id,
                MapId = character.Map.Id,
                X = character.Position.X,
                Y = character.Position.Y,
                Music = 1,
                Direction = character.Direction
            };
        }

        public static Lev CreateLevPacket(this Character character)
        {
            return new Lev
            {
                Level = character.Level,
                Experience = 0,
                RequiredExperience = 1,
                JobLevel = character.JobLevel,
                JobExperience = 0,
                RequiredJobExperience = 1,
                HeroLevel = character.HeroLevel,
                HeroExperience = 0,
                RequiredHeroExperience = 1,
                Reputation = 0,
                Cp = 0
            };
        }

        public static CInfo CreateCInfoPacket(this Character character)
        {
            return new CInfo
            {
                Name = character.Name,
                CharacterId = character.Id,
                AuthorityType = AuthorityType.GameMaster,
                Gender = character.Gender,
                HairStyle = character.HairStyle,
                HairColor = character.HairColor,
                Job = character.Job,
                Icon = 1,
                Compliment = 0,
                Morph = 0,
                Invisible = false,
                FamilyLevel = 0,
                MorphUpgrade = 0,
                ArenaWinner = false
            };
        }

        public static Fd CreateFdPacket(this Character character)
        {
            var dignityIcon = character.Dignity >= -99 ? DignityIcon.Default
                : character.Dignity >= -200 ? DignityIcon.Dubious
                : character.Dignity >= -400 ? DignityIcon.Dreadful
                : character.Dignity >= -600 ? DignityIcon.Unqualified
                : character.Dignity >= -800 ? DignityIcon.Useless 
                : DignityIcon.Failed;
            
            return new Fd
            {
                Reputation = character.Reputation,
                RepucationIcon = character.ReputationIcon,
                Dignity = character.Dignity,
                DignityIcon = dignityIcon
            };
        }

        public static CList CreateCList(this CharacterDTO character)
        {
            return new CList
            {
                Name = character.Name,
                Slot = character.Slot,
                HairColor = character.HairColor,
                HairStyle = character.HairStyle,
                Level = character.Level,
                Gender = character.Gender,
                HeroLevel = 0,
                JobLevel = character.JobLevel,
                Job = character.Job,
                Equipments = Enumerable.Range(0, 10).Select(x => (short?) null),
                Pets = Enumerable.Range(0, 26).Select(x => (short?) null),
                QuestCompletion = 1,
                QuestPart = 1,
                Rename = false
            };
        }

        public static Tit CreateTit(this Character character)
        {
            return new Tit
            {
                ClassName = 35,
                Name = character.Name
            };
        }
    }
}