using UnityEngine;

namespace Ingame.SaveLoad
{
    public interface ISerializer
    {
        public string SerializeData<T>(T saveData);
        public T DeserializeData<T>(string serializedSaveData) where T : class;
    }
    
    public sealed class JsonSerializer : ISerializer
    {
        public string SerializeData<T>(T saveData)
        {
            var json = JsonUtility.ToJson(saveData);
            return json;
        }
        
        public T DeserializeData<T>(string serializedSaveData) where T : class
        {
            if (string.IsNullOrWhiteSpace(serializedSaveData))
                return null;
            
            var result = JsonUtility.FromJson<T>(serializedSaveData);
            
            return result;
        }
    }
}