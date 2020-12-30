using System.Collections.Generic;
using System.IO;
using System.Linq;
using YamlDotNet.Serialization;

namespace SharperMC.Core.Config.Impl.Yaml
{
    public class YamlConfig : MemorySection, IConfig, ISection
    {
        private string _filePath;

        public static void Test()
        {
            IConfig config = new YamlConfig("/media/shoghi/Data/appdata/projects/C#/SharperMC/TestServer/Logs/test.yml");
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

        public YamlConfig(string filePath, bool start = true)
        {
            Name = filePath.Substring(filePath.LastIndexOf(Path.DirectorySeparatorChar));
            _filePath = filePath;
            if (start) Reload();
        }

        public string GetFilePath()
        {
            return _filePath;
        }

        public void Save()
        {
            var serializer = new Serializer();
            var mainDict = new Dictionary<string, object>();
            
            foreach (var (key, value) in Dict)
            {
                switch (value)
                {
                    case ISection v:
                        ConvertToDict((IDictionary<string, object>) (mainDict[key] = v.GetKeys()));
                        break;
                    case List<object> objects:
                    {
                        for (var index = 0; index < objects.Count; index++)
                            if (objects[index] is ISection sect)
                                ConvertToDict((IDictionary<string, object>) (objects[index] = sect.GetKeys()));
                        mainDict[key] = objects;
                        break;
                    }
                    default:
                        mainDict[key] = value;
                        break;
                }
            }

            System.Console.WriteLine(new string('=', 20));
            // System.Console.WriteLine(serializer.Serialize(mainDict));
            File.WriteAllText(_filePath, serializer.Serialize(mainDict));
        }

        private void ConvertToDict(IDictionary<string, object> dict)
        {
            foreach (var (key, value) in new Dictionary<string, object>(dict))
            {
                switch (value)
                {
                    case ISection v:
                        ConvertToDict((IDictionary<string, object>) (dict[key] = v.GetKeys()));
                        break;
                    case List<object> objects:
                    {
                        for (var index = 0; index < objects.Count; index++)
                            if (objects[index] is ISection sect)
                                ConvertToDict((IDictionary<string, object>) (objects[index] = sect.GetKeys()));
                        break;
                    }
                }
            }
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
        
        public override ISection NewSection(string key, Dictionary<string, object> dict)
        {
            return new YamlSection(key, dict);
        }
    }
}