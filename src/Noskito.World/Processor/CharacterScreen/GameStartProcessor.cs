using System.Threading.Tasks;
using Noskito.World.Packet.Client.CharacterScreen;
using Noskito.World.Processor.Extension;

namespace Noskito.World.Processor.CharacterScreen
{
    public class GameStartProcessor : PacketProcessor<GameStart>
    {
        protected override async Task Process(WorldSession session, GameStart packet)
        {
            var map = session.Character.Map;

            await session.SendTit();
            await session.SendFd();
            await session.SendCInfo();
            await session.SendLev();
            await session.SendStat();

            await session.SendAt();
            await session.SendCMap(joining: true);

            map.AddEntity(session.Character);

            await map.BroadcastIn(session.Character);
            
            foreach (var monster in map.Monsters)
            {
                await session.SendIn(monster);
            }

            foreach (var character in map.Characters)
            {
                await session.SendIn(character);
            }
        }
    }
}