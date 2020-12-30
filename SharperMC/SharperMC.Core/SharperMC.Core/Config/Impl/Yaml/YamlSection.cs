using System.Collections.Generic;

namespace SharperMC.Core.Config.Impl.Yaml
{
    public class YamlSection : MemorySection
    {
        public YamlSection(string name, Dictionary<string, object> dict) : base(name, dict)
        {
        }

        public YamlSection(string name, Dictionary<object, object> dict)
        {
            Name = name;
            Dict = new Dictionary<string, object>();
            foreach(var (key, value) in dict)
            {
                if (!(key is string)) continue;
                var sk = (string) key;
                switch (value)
                {
                    case Dictionary<object, object> d:
                        Dict[sk] = new YamlSection(sk, d);
                        break;
                    case List<object> list:
                        for (var index = 0; index < list.Count; index++)
                            if (list[index] is Dictionary<object, object> dictionary)
                                list[index] = new YamlSection(sk, dictionary);
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
            return new YamlSection(key, dict);
        }
    }
}