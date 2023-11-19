namespace Noskito.Modules
{
    public interface IModuleManager
    {
        IModule[] LoadPlugin(FileInfo file);

        IModule[] LoadPlugins(DirectoryInfo directory);
    }
}