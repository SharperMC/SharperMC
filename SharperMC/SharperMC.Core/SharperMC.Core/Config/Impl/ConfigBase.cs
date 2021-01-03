using System.Collections.Generic;
using System.IO;

namespace SharperMC.Core.Config.Impl
{
    public abstract class ConfigBase : MemorySection, IConfig
    {
        protected string _filePath;

        public ConfigBase(string filePath, bool start = true)
        {
            Name = filePath.Substring(filePath.LastIndexOf(Path.DirectorySeparatorChar));
            _filePath = filePath;
            if (start) Reload();
            else Dict = new Dictionary<string, object>();
        }

        public ConfigBase(ISection section, string filePath)
        {
            Name = filePath.Substring(filePath.LastIndexOf(Path.DirectorySeparatorChar));
            _filePath = filePath;
            Dict = new Dictionary<string, object>(section.GetKeys());
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


            File.WriteAllText(_filePath, SerializeToString(mainDict));
        }

        protected static void ConvertToDict(IDictionary<string, object> dict)
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
            Dict = Deserialize();
            foreach (var (key, value) in new Dictionary<string, object>(Dict))
                switch (value)
                {
                    // I must ask, WHY IS THERE NO <?> IN C# WTF
                    case Dictionary<object, object> objects:
                        Dict[key] = new ConfigSection(key, objects);
                        break;
                    case Dictionary<string, object> objects:
                        Dict[key] = new ConfigSection(key, objects);
                        break;
                    case List<object> objects:
                        for (var index = 0; index < objects.Count; index++)
                            objects[index] = objects[index] switch
                            {
                                Dictionary<object, object> dictionary => new ConfigSection(key, dictionary),
                                Dictionary<string, object> dictionary => new ConfigSection(key, dictionary),
                                _ => objects[index]
                            };
                        break;
                }
        }

        protected abstract string SerializeToString(Dictionary<string, object> dict);
        protected abstract Dictionary<string, object> Deserialize();
        
        public override ISection NewSection(string key, Dictionary<string, object> dict)
        {
            return new ConfigSection(key, dict);
        }
        
        // ??? Why do you do make me do this C#
        public object Remove(string key)
        {
            return base.Remove(key);
        }

        public void Set(string key, object value)
        {
            base.Set(key, value);
        }
    }
}