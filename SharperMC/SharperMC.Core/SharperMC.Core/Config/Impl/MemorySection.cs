using System;
using System.Collections.Generic;

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

        public MemorySection(ISection section, string name = null)
        {
            Name = name ?? section.GetName();
            Dict = new Dictionary<string, object>(section.GetKeys()); // shallow copy
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

        public IDictionary<string, object> GetKeys()
        {
            return Dict;
        }

        public object Remove(string key)
        {
            if (!key.Contains("."))
            {
                if (!Dict.TryGetValue(key, out var v0)) return null;
                Dict.Remove(key);
                return v0;
            }

            var k = key.Substring(0, key.IndexOf(".", StringComparison.Ordinal));
            if (!Dict.TryGetValue(k, out var v)) return null;
            if (v is ISection section)
                section.Remove(key.Substring(key.IndexOf(".", StringComparison.Ordinal) + 1));
            return null;
        }

        public void Set(string key, object value)
        {
            if (key.Contains("."))
            {
                var k = key.Substring(0, key.IndexOf(".", StringComparison.Ordinal));
                if (Dict.TryGetValue(k, out var v))
                {
                    if (v is ISection section)
                    {
                        section.Set(key.Substring(key.IndexOf(".", StringComparison.Ordinal) + 1), value);
                    }
                    else
                        throw new ArgumentException($"{k} is not a memory section!");
                }
                else
                {
                    ISection section;
                    Dict[k] = section = NewSection(k, new Dictionary<string, object>());
                    section.Set(key.Substring(key.IndexOf(".", StringComparison.Ordinal) + 1), value);
                }
            }
            else Dict[key] = value;
        }


        /// <summary>
        /// Generates a new ISection.
        /// 
        /// Override this if you want to generate something other than a MemorySection.
        /// </summary>
        /// <param name="key">The section's key.</param>
        /// <param name="dict">The section's dict.</param>
        /// <returns>A new ISection</returns>
        public virtual ISection NewSection(string key, Dictionary<string, object> dict)
        {
            return new MemorySection(key, dict);
        }
    }
}