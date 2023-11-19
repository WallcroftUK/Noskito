using System;
using System.Text;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Noskito.Logging;

namespace Noskito.Login.Network.Pipeline
{
    public class Encoder : MessageToByteEncoder<string>
    {
        protected override void Encode(IChannelHandlerContext context, string message, IByteBuffer output)
        {
            if (!output.IsWritable())
            {
                Log.Debug("Output is not writable");
                return;
            }

            if (string.IsNullOrEmpty(message))
            {
                Log.Debug("Can't encode an empty or null string");
                return;
            }

            message += " ";

            var tmp = Encoding.UTF8.GetBytes(message);
            for (var i = 0; i < message.Length; i++) tmp[i] = Convert.ToByte(tmp[i] + 15);

            tmp[^1] = 25;

            output.WriteBytes(tmp);
        }
    }
}