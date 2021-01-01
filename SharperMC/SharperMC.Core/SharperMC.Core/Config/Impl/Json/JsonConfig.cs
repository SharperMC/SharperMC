using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace SharperMC.Core.Config.Impl.Json
{
    public class JsonConfig : MemorySection, IConfig, ISection
    {
        private string _filePath;
        private Formatting _formatting;

        public static void Test()
        {
            // please ignore the path thx
            IConfig config =
                new JsonConfig("/media/shoghi/Data/appdata/projects/C#/SharperMC/TestServer/Logs/test.json");
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

        public JsonConfig(string filePath, Formatting formatting = Formatting.None, bool start = true)
        {
            Name = filePath.Substring(filePath.LastIndexOf(Path.DirectorySeparatorChar));
            _filePath = filePath;
            _formatting = formatting;
            if (start) Reload();
            else Dict = new Dictionary<string, object>();
        }

        public string GetFilePath()
        {
            return _filePath;
        }

        public void Save()
        {
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

            File.WriteAllText(_filePath, JsonConvert.SerializeObject(mainDict, _formatting));
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
            Dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(File.OpenText(_filePath).ReadToEnd());
            foreach (var (key, value) in new Dictionary<string, object>(Dict))
                switch (value)
                {
                    case Dictionary<object, object> objects:
                        Dict[key] = new ConfigSection(key, objects);
                        break;
                    case List<object> objects:
                        for (var index = 0; index < objects.Count; index++)
                            if (objects[index] is Dictionary<object, object> dictionary)
                                objects[index] = new ConfigSection(key, dictionary);
                        break;
                }
        }

        public override ISection NewSection(string key, Dictionary<string, object> dict)
        {
            return new ConfigSection(key, dict);
        }
    }
}