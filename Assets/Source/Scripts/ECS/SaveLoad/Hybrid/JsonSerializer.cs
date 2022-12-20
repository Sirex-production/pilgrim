using System;
using UnityEngine;

namespace Ingame.SaveLoad
{
    public interface ISerializer
    {
        public string SerializeData<T>(T saveData);
        public T DeserializeData<T>(string serializedSaveData) where T : class;
        public dynamic DynamicDeserializeTo(string serializedSaveData, Type type);
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

        public dynamic DynamicDeserializeTo(string serializedSaveData, Type type)
        {
            var method = this.GetType().GetMethod("DeserializeData");
            if (method == null) return null;
            var generic = method.MakeGenericMethod(type);
            var result = generic.Invoke(this, new object[] {serializedSaveData});
            return result;

        }
    }
}