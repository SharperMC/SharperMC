using System.Collections.Generic;
using System.IO;
using System.Linq;
using YamlDotNet.Serialization;

namespace SharperMC.Core.Config.Impl.Yaml
{
    public class YamlConfig : MemorySection, IConfig, ISection
    {
        private string _filePath;

        private static void Test()
        {
            IConfig config = new YamlConfig("/media/shoghi/Data/appdata/projects/C#/SharperMC/TestServer/Logs/test.yml");
            System.Console.WriteLine(config.Get("test.owo") ?? "null");
            System.Console.WriteLine(config.Get("uwu") ?? "null");
            System.Console.WriteLine(config.Get("hot.amazing.owo") ?? "null");
            System.Console.WriteLine(config.GetISections("hot.amazing.owo").ToArray()[2].Get("jesus"));
            System.Console.WriteLine(config.Get("hot.amazing.uwu") ?? "null");
        }
        
        public YamlConfig(string filePath)
        {
            Name = filePath.Substring(filePath.LastIndexOf(Path.DirectorySeparatorChar));
            _filePath = filePath;
            Reload();
        }

        public string GetFilePath()
        {
            return _filePath;
        }

        public void Save()
        {
            throw new System.NotImplementedException("Todo: make this"); // Todo: Make this
        }

        public void Reload()
        {
            var deserializer = new Deserializer();
            Dict = deserializer.Deserialize<Dictionary<string, object>>(File.OpenText(_filePath).ReadToEnd());
            foreach(var (key, value) in new Dictionary<string, object>(Dict))
                switch (value)
                {
                    case Dictionary<object, object> objects:
                        Dict[key] = new YamlSection(key, objects);
                        break;
                    case List<object> objects:
                        for (var index = 0; index < objects.Count; index++)
                        {
                            var o = objects[index];
                            if (o is Dictionary<object, object> dictionary)
                                objects[index] = new YamlSection(key, dictionary);
                        }

                        break;
                }
        }
    }
}