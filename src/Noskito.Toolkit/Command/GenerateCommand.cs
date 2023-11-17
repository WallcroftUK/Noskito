using CommandLine;

namespace Noskito.Toolkit.Command
{
    [Verb("generate", HelpText = "Generate yml files from standard files")]
    public class GenerateCommand
    {
        [Option('i', "input", Required = true, HelpText = "Path to folder used for generation")]
        public string Input { get; set; }
        
        [Option('o', "output", Required = true, HelpText = "Path to folder where yml files will be generated")]
        public string Output { get; set; }
    }
}