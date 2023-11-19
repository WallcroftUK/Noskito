using System;
using System.Threading.Tasks;
using Noskito.Database.Dto.Accounts;
using Noskito.World.Game.Entities;
using Noskito.World.Network;
using Noskito.World.Packet.Server;

namespace Noskito.World
{
    public class WorldSession
    {
        private readonly NetworkClient _client;

        public WorldSession(NetworkClient client)
        {
            _client = client;
        }

        public Guid Id { get; } = Guid.NewGuid();

        public AccountDTO Account { get; set; }
        public Character Character { get; set; }

        public int Key
        {
            get => _client.EncryptionKey;
            set => _client.EncryptionKey = value;
        }

        public Task SendPacket<T>(T packet) where T : SPacket
        {
            return _client.SendPacket(packet);
        }

        public Task Disconnect()
        {
            return _client.Disconnect();
        }
    }
}
