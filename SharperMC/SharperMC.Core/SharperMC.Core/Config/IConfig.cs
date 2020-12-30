namespace SharperMC.Core.Config
{
    // Yaml : almost done, just need to implement serialization
    // Json : todo
    // other? (toml...)
    public interface IConfig : ISection
    {
        public string GetFilePath();
        public void Save();
        public void Reload();
    }
}