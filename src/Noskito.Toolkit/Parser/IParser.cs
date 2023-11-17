using System.IO;
using System.Threading.Tasks;

namespace Noskito.Toolkit.Parser
{
    public interface IParser
    {
        Task Parse(DirectoryInfo directory);
    }
}