using System.Collections.Generic;
using System.Linq;

namespace SharperMC.Core.Config
{
    public interface ISection
    {
        /// <summary>
        /// Gets name.
        /// </summary>
        /// <returns>Name</returns>
        public string GetName();

        /// <summary>
        /// Gets a value in the section (split by a period)
        /// </summary>
        /// <param name="key">The key of the value.</param>
        /// <param name="defaultValue">The value to return if not found.</param>
        /// <returns>The value or defaultValue if not found</returns>
        public object Get(string key, object defaultValue = null);
        
        public IDictionary<string, object> GetKeys();
        
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

        /// <summary>
        /// Removes a value from the section and returns it. If the value is not found, returns null.
        /// </summary>
        /// <param name="key">The key of the value.</param>
        /// <returns>The value or null if not found</returns>
        public object Remove(string key);

        /// <summary>
        /// Sets a value in the section.
        /// </summary>
        /// <param name="key">The key of the value.</param>
        /// <param name="value">The value itself.</param>
        /// <exception cref="System.ArgumentException">If a key is found to not be a section.</exception>
        public void Set(string key, object value);
        
        /// <summary>
        /// Generates a new ISection.
        /// </summary>
        /// <param name="key">The section's key.</param>
        /// <param name="dict">The section's dict.</param>
        /// <returns>A new ISection</returns>
        public ISection NewSection(string key, Dictionary<string, object> dict);
    }
}