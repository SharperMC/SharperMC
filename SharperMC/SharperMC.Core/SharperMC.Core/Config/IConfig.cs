namespace SharperMC.Core.Config
{
    // Yaml : Done
    // Json : Done
    // other? (toml...)
    public interface IConfig : ISection
    {
        public string GetFilePath();
        public void Save();
        public void Reload();
    }
}