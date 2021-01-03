using System.Collections.Generic;
using System.Linq;

namespace SharperMC.Core.Config.Impl
{
    public class ConfigSection : MemorySection
    {
        public ConfigSection(string name, IDictionary<string, object> dict)
        {
            var d = new Dictionary<object, object>(); // Casting didn't wanna play nice.
            foreach (var (key, value) in dict) d.Add(key, value);
            Setup(name, d);
        }

        public ConfigSection(string name, IDictionary<object, object> dict)
        {
            Setup(name, dict);
        }

        public ConfigSection(ISection section, string name = null) : this(name ?? section.GetName(), section.GetKeys())
        {
        }

        private void Setup(string name, object obj)
        {
            var dict = obj as IDictionary<object, object>;
            Name = name;
            Dict = new Dictionary<string, object>();
            foreach(var (key, value) in dict)
            {
                if (!(key is string)) continue;
                var sk = (string) key;
                switch (value)
                {
                    case Dictionary<object, object> d:
                        Dict[sk] = new ConfigSection(sk, d);
                        break;
                    case Dictionary<string, object> d:
                        Dict[sk] = new ConfigSection(sk, d);
                        break;
                    case List<object> list:
                        for (var index = 0; index < list.Count; index++)
                            list[index] = list[index] switch
                            {
                                Dictionary<object, object> dictionary => new ConfigSection(sk, dictionary),
                                Dictionary<string, object> dictionary => new ConfigSection(sk, dictionary),
                                _ => list[index]
                            };
                        Dict[sk] = list;
                        break;
                    default:
                        Dict[sk] = value;
                        break;
                }
            }
        }

        public override ISection NewSection(string key, Dictionary<string, object> dict)
        {
            return new ConfigSection(key, dict);
        }
    }
}