namespace DataLoader.Module
{
    public class RsrcConfigLoader
    {
        public RsrcConfigLoader(string basePath) => BasePath = basePath;

        public string BasePath { get; }

        public string DataPath => Path.Combine(BasePath, "dat");
        public string LangPath => Path.Combine(BasePath, "lang");
    }
}