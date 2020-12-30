using System;
using System.Collections.Generic;
using System.Linq;

namespace SharperMC.Core.Config.Impl
{
    public class MemorySection : ISection
    {
        internal string Name;
        internal Dictionary<string, object> Dict;

        public MemorySection(string name, Dictionary<string, object> dict)
        {
            Name = name;
            Dict = dict;
        }

        internal MemorySection()
        {
        }

        public string GetName()
        {
            return Name;
        }

        public object Get(string key, object defaultValue = null)
        {
            if (key.Contains("."))
            {
                Dict.TryGetValue(key.Substring(0, key.IndexOf(".", StringComparison.Ordinal)), out var value);
                return value is ISection section
                    ? section.Get(key.Substring(key.IndexOf(".", StringComparison.Ordinal) + 1))
                    : defaultValue;
            }
            else return Dict.TryGetValue(key, out var value) ? value : defaultValue;
        }

        public void Set(string key, object value)
        {
            Dict[key] = value;
        }
    }
}