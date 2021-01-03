using System.Collections.Generic;
using System.IO;
using System.Linq;
using YamlDotNet.Serialization;

namespace SharperMC.Core.Config.Impl.Yaml
{
    public class YamlConfig : ConfigBase, ISection, IConfig
    {
        public static void Test()
        {
            IConfig config =
                new YamlConfig("/media/shoghi/Data/appdata/projects/C#/SharperMC/TestServer/Logs/test.yml");
            System.Console.WriteLine(config.Get("test.owo") ?? "null");
            System.Console.WriteLine(config.Get("uwu") ?? "null");
            System.Console.WriteLine(config.Get("hot.amazing.owo") ?? "null");
            System.Console.WriteLine(config.GetISections("hot.amazing.owo").ToArray()[2].Get("jesus"));
            System.Console.WriteLine(config.Get("hot.amazing.uwu") ?? "null");
            config.Set("hot.amazing.gj", 13);
            config.Set("hot.amazing.gjwd", "\'\"noice\"\'");
            config.Remove("test.owo");
            config.Save();
        }

        public YamlConfig(string filePath, bool start = true) : base(filePath, start)
        {
        }

        public YamlConfig(ISection section, string filePath) : base(section, filePath)
        {
        }

        protected override string SerializeToString(Dictionary<string, object> dict)
        {
            return new Serializer().Serialize(dict);
        }

        protected override Dictionary<string, object> Deserialize()
        {
            return new Deserializer().Deserialize<Dictionary<string, object>>(File.OpenText(_filePath).ReadToEnd());
        }
    }
}