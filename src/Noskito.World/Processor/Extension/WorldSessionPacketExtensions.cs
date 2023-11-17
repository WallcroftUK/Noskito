using System.Collections.Generic;
using System.Threading.Tasks;
using Noskito.Database.Dto;
using Noskito.World.Game.Entities;
using Noskito.World.Packet.Server.CharacterScreen;
using Noskito.World.Processor.Extension.Generator;

namespace Noskito.World.Processor.Extension
{
    public static class WorldSessionPacketExtensions
    {
        public static Task SendTit(this WorldSession session)
        {
            return session.SendPacket(session.Character.CreateTit());
        }

        public static async Task SendCList(this WorldSession session, IEnumerable<CharacterDTO> characters)
        {
            await session.SendPacket(new CListStart());
            foreach (var character in characters)
            {
                await session.SendPacket(character.CreateCList());
            }
            await session.SendPacket(new CListEnd());
        }

        public static Task SendFd(this WorldSession session)
        {
            return session.SendPacket(session.Character.CreateFdPacket());
        }

        public static Task SendCInfo(this WorldSession session)
        {
            return session.SendPacket(session.Character.CreateCInfoPacket());
        }

        public static Task SendLev(this WorldSession session)
        {
            return session.SendPacket(session.Character.CreateLevPacket());
        }

        public static Task SendAt(this WorldSession session)
        {
            return session.SendPacket(session.Character.CreateAtPacket());
        }

        public static Task SendCMap(this WorldSession session, bool joining)
        {
            return session.SendPacket(session.Character.Map.CreateCMapPacket(joining));
        }

        public static Task SendStat(this WorldSession session)
        {
            return session.SendPacket(session.Character.CreateStatPacket());
        }

        public static Task SendIn(this WorldSession session, Monster entity)
        {
            return session.SendPacket(entity.CreateInPacket());
        }

        public static Task SendIn(this WorldSession session, Character entity)
        {
            if (session.Id == entity.Session.Id)
            {
                return Task.CompletedTask;
            }
            
            return session.SendPacket(entity.CreateInPacket());
        }
    }
}