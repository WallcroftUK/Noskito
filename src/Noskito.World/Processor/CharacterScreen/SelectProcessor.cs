using System.Threading.Tasks;
using Noskito.Database.Repository;
using Noskito.World.Game;
using Noskito.World.Game.Entities;
using Noskito.World.Game.Maps;
using Noskito.World.Game.Services;
using Noskito.World.Packet.Client.CharacterScreen;
using Noskito.World.Packet.Server.CharacterScreen;

namespace Noskito.World.Processor.CharacterScreen
{
    public class SelectProcessor : PacketProcessor<Select>
    {
        private readonly MapManager mapManager;
        private readonly ReputationService reputationService;
        private readonly CharacterRepository characterRepository;

        public SelectProcessor(MapManager mapManager, CharacterRepository characterRepository, ReputationService reputationService)
        {
            this.mapManager = mapManager;
            this.characterRepository = characterRepository;
            this.reputationService = reputationService;
        }

        protected override async Task Process(WorldSession session, Select packet)
        {
            if (session.Account is null)
            {
                await session.Disconnect();
                return;
            }

            var character = await characterRepository.GetCharacterInSlot(session.Account.Id, packet.Slot);
            if (character is null)
            {
                await session.Disconnect();
                return;
            }

            session.Character = new Character(session)
            {
                Id = character.Id,
                Name = character.Name,
                Job = character.Job,
                HairColor = character.HairColor,
                HairStyle = character.HairStyle,
                Gender = character.Gender,
                Level = character.Level,
                JobLevel = character.JobLevel,
                HeroLevel = character.HeroLevel,
                Experience = character.Experience,
                JobExperience = character.JobExperience,
                HeroExperience = character.HeroExperience,
                Direction = character.Direction,
                Position = new Position
                {
                    X = character.X, 
                    Y = character.Y
                },
                Hp = character.Hp,
                Mp = character.Mp,
                MaxHp = 300,
                MaxMp = 150,
                Reputation = character.Reputation,
                Dignity = character.Dignity,
                Speed = 10,
                ReputationIcon = await reputationService.GetIcon(character.Reputation),
                Map = await mapManager.GetMap(character.MapId)
            };

            await session.SendPacket(new Ok());
        }
    }
}