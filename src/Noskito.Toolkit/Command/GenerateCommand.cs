using CommandLine;

namespace Noskito.Toolkit.Command
{
    [Verb("generate", HelpText = "Generate yml files from standard files")]
    public class GenerateCommand
    {
        [Option('p', "path", Required = true, HelpText = "Path to folder used for generation")]
        public string Path { get; set; }
        
    }
}