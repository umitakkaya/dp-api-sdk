using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpApiSDK
{
    public static class DpSerializer<T>
    {
        public static string Serialize(T value)
        {
            return JsonConvert.SerializeObject(value, Globals.SerializerSettings);
        }

        public static T Deserialize(string value)
        {
            return JsonConvert.DeserializeObject<T>(value, Globals.SerializerSettings);
        }
    }
}
