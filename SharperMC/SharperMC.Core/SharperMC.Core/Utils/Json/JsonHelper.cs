using System.Linq;
using Newtonsoft.Json.Linq;

namespace SharperMC.Core.Utils.Json
{

    public static class JsonHelper
    {
        public static object Deserialize(string json)
        {
            return ToObject(JToken.Parse(json));
        }

        private static object ToObject(JToken token)
        {
            return token.Type switch
            {
                JTokenType.Object => token.Children<JProperty>()
                    .ToDictionary(prop => prop.Name, prop => ToObject(prop.Value)),
                JTokenType.Array => token.Select(ToObject).ToList(),
                _ => ((JValue) token).Value
            };
        }
    }

}