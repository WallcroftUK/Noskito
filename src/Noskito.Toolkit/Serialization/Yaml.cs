using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Noskito.Toolkit.Serialization
{
    public class Yaml : ISerialization
    {
        private readonly IDeserializer deserializer;
        private readonly ISerializer serializer;

        public Yaml()
        {
            deserializer = new DeserializerBuilder().WithNamingConvention(PascalCaseNamingConvention.Instance).Build();
            serializer = new SerializerBuilder().WithNamingConvention(PascalCaseNamingConvention.Instance).Build();
        }

        public void Serialize(TextWriter writer, object value)
        {
            serializer.Serialize(writer, value);
        }

        public T Deserialize<T>(TextReader reader)
        {
            return deserializer.Deserialize<T>(reader);
        }
    }
}