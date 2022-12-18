using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using ModestTree;

namespace Ingame.SaveLoad
{
    public static class BinarySerializer
    {
        public static string SerializeData<T>(T saveData)
        {
            using var stream = new MemoryStream();
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, saveData);

            return Convert.ToBase64String(stream.ToArray());
        }


        public static T DeserializeData<T>(string serializedSaveData) where T : class
        {
            if (serializedSaveData == null || serializedSaveData.IsEmpty())
            {
                return null;
            }
            var serializedBytesData = Convert.FromBase64String(serializedSaveData);

            using var stream = new MemoryStream(serializedBytesData);
            var formatter = new BinaryFormatter();
            var deserializedSaveData = formatter.Deserialize(stream);

            return (T) deserializedSaveData;
        }
    }
}