using System.IO;

namespace Noskito.Toolkit.Generator
{
    public interface IGenerator
    {
        void Generate(DirectoryInfo input, DirectoryInfo output);
    }
}