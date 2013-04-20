using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

namespace GoogleMusicAPI
{
    public static class JSON
    {
        public static T DeserializeObject<T>(string data)
        {
            return Deserialize<T>(data);
        }

        public static T Deserialize<T>(string data)
        {
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(data));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            return (T)serializer.ReadObject(ms);
        }

        public static string Serialize<T>(T data)
        {
            MemoryStream ms = new MemoryStream();
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            serializer.WriteObject(ms, data);
            return Encoding.UTF8.GetString(ms.ToArray(), 0, (int)ms.Length);
        }
    }
}
