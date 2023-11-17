using System.Linq;
using System.Threading.Tasks;
using Noskito.Database.Dto;
using Noskito.Database.Repository;
using Noskito.Enum;
using Noskito.World.Packet.Client.CharacterScreen;
using Noskito.World.Packet.Server.CharacterScreen;
using Noskito.World.Processor.Extension;

namespace Noskito.World.Processor.CharacterScreen
{
    public class CharNewProcessor : PacketProcessor<CharNew>
    {
        private readonly CharacterRepository characterRepository;

        public CharNewProcessor(CharacterRepository characterRepository)
        {
            this.characterRepository = characterRepository;
        }

        protected override async Task Process(WorldSession session, CharNew packet)
        {
            if (session.Account is null) return;

            var slotTaken = await characterRepository.IsSlotTaken(session.Account.Id, packet.Slot);
            if (slotTaken)
            {
                await session.Disconnect();
                return;
            }

            var nameTaken = await characterRepository.IsNameTaken(packet.Name);
            if (nameTaken)
            {
                await session.Disconnect();
                return;
            }

            await characterRepository.Save(new CharacterDTO
            {
                Name = packet.Name,
                Slot = packet.Slot,
                AccountId = session.Account.Id,
                Level = 1,
                JobLevel = 1,
                HeroLevel = 0,
                Job = Job.Adventurer,
                Gender = packet.Gender,
                HairColor = packet.HairColor,
                HairStyle = packet.HairStyle,
                Dignity = 0,
                Reputation = 0,
                Experience = 0,
                JobExperience = 0,
                HeroExperience = 0,
                MapId = 1,
                X = 75,
                Y = 115,
                Direction = Direction.South,
                Hp = 300,
                Mp = 150,
            });

            await session.SendPacket(new Success());

            var characters = await characterRepository.FindAll(session.Account.Id);
            
            await session.SendCList(characters);
        }
    }
}