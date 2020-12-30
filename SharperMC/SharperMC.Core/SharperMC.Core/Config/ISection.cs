using System.Collections.Generic;
using System.Linq;

namespace SharperMC.Core.Config
{
    public interface ISection
    {
        public string GetName();
        
        public object Get(string key, object defaultValue=null);

        public virtual string GetString(string key, string defaultValue = "")
        {
            return Get(key, defaultValue) as string ?? defaultValue;
        }

        public virtual bool GetBoolean(string key, bool defaultValue = false)
        {
            return (bool) Get(key, defaultValue);
        }

        public virtual int GetInt(string key, int defaultValue = 0)
        {
            return (int) Get(key, defaultValue);
        }

        public virtual float GetFloat(string key, float defaultValue = 0.0f)
        {
            return (float) Get(key, defaultValue);
        }

        public virtual ISection GetSection(string key)
        {
            return Get(key) as ISection;
        }

        public virtual IEnumerable<object> GetList(string key)
        {
            return Get(key, new object[0]) as IEnumerable<object> ?? new object[0];
        }

        public virtual IEnumerable<string> GetStrings(string key)
        {
            return GetList(key).Cast<string>();
        }

        public virtual IEnumerable<bool> GetBools(string key)
        {
            return GetList(key).Cast<bool>();
        }

        public virtual IEnumerable<int> GetInts(string key)
        {
            return GetList(key).Cast<int>();
        }

        public virtual IEnumerable<float> GetFloats(string key)
        {
            return GetList(key).Cast<float>();
        }

        public virtual IEnumerable<ISection> GetISections(string key)
        {
            return GetList(key).Cast<ISection>();
        }

        public void Set(string key, object value);
    }
}