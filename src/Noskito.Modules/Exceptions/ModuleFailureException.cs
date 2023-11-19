namespace Noskito.Modules.Exceptions
{
    public class ModuleException : Exception
    {
        public IModule Module { get; }

        public ModuleException(IModule module, string message = "Module Exception")
            : base($"[{module.Name}] {message}")
        {
            Module = module;
        }
    }
}