using System.IO;

namespace Noskito.Toolkit.Serialization
{
    public interface ISerialization
    {
        void Serialize(TextWriter writer, object value);
        T Deserialize<T>(TextReader reader);
    }
}