namespace Noskito.Modules
{
    public class ModulePathConfigurator : IModulePathConfigurator
    {
        public ModulePathConfigurator(string modulesPath) => ModulesPath = modulesPath;

        public string ModulesPath { get; }
    }
}