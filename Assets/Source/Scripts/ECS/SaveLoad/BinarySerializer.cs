using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Ingame.SaveLoad
{
    public static class BinarySerializer
    {
        public static string SerializeData(SaveDataContainer saveData)
        {
            using var stream = new MemoryStream();
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, saveData);

            return Convert.ToBase64String(stream.ToArray());
        }


        public static SaveDataContainer DeserializeData(string serializedSaveData)
        {
            var serializedBytesData = Convert.FromBase64String(serializedSaveData);

            using var stream = new MemoryStream(serializedBytesData);
            var formatter = new BinaryFormatter();
            var deserializedSaveData = formatter.Deserialize(stream);

            return (SaveDataContainer) deserializedSaveData;
        }
    }
}