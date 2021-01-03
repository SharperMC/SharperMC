using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharperMC.Core.Utils.Json;

namespace SharperMC.Core.Config.Impl.Json
{
    public class JsonConfig : ConfigBase
    {
        private Formatting _formatting;

        public static void Test()
        {
            // please ignore the path thx
            IConfig config =
                new JsonConfig("/media/shoghi/Data/appdata/projects/C#/SharperMC/TestServer/Logs/test.json");
            System.Console.WriteLine(config.Get("test.owo") ?? "null");
            System.Console.WriteLine(config.Get("uwu") ?? "null");
            System.Console.WriteLine(config.Get("hot") ?? "null");
            System.Console.WriteLine(config.Get("hot.amazing") ?? "null");
            System.Console.WriteLine(string.Join(", ", config.GetSection("hot.amazing").GetKeys().Keys));
            System.Console.WriteLine(config.Get("hot.amazing.owo") ?? "null");
            System.Console.WriteLine(config.GetISections("hot.amazing.owo").ToArray()[2].Get("jesus"));
            System.Console.WriteLine(config.Get("hot.amazing.uwu") ?? "null");
            config.Set("hot.amazing.gj", 13);
            config.Set("hot.amazing.gjwd", "\'\"noice\"\'");
            config.Remove("test.owo");
            config.Save();
        }

        public JsonConfig(string filePath, Formatting formatting = Formatting.None, bool start = true) : base(filePath,
            start)
        {
            _formatting = formatting;
        }

        public JsonConfig(ISection section, string filePath, Formatting formatting = Formatting.None) : base(section,
            filePath)
        {
            _formatting = formatting;
        }

        protected override string SerializeToString(Dictionary<string, object> dict)
        {
            return JsonConvert.SerializeObject(dict, _formatting);
        }

        protected override Dictionary<string, object> Deserialize()
        {
            return (Dictionary<string, object>) JsonHelper.Deserialize(File.OpenText(_filePath).ReadToEnd());
        }

        public override ISection NewSection(string key, Dictionary<string, object> dict)
        {
            return new ConfigSection(key, dict);
        }
    }
}