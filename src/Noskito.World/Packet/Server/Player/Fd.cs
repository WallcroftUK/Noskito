using Noskito.Enum;
using Noskito.World.Packet.Extension;

namespace Noskito.World.Packet.Server.Player
{
    public class Fd : SPacket
    {
        public long Reputation { get; init; }
        public ReputationIcon RepucationIcon { get; init; }
        public int Dignity { get; init; }
        public DignityIcon DignityIcon { get; init; }
    }

    public class FdCreator : SPacketCreator<Fd>
    {
        protected override string CreatePacket(Fd source)
        {
            return "fd " +
                   $"{source.Reputation} " +
                   $"{source.RepucationIcon.Format()} " +
                   $"{source.Dignity} " +
                   $"{source.DignityIcon.Format()}";
        }
    }
}