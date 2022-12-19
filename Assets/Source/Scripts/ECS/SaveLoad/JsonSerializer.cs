using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using ModestTree;
using Newtonsoft.Json;
using UnityEngine;

namespace Ingame.SaveLoad
{
    public static class JsonSerializer
    {
        public static string SerializeData<T>(T saveData)
        {
            var json = JsonUtility.ToJson(saveData);
            return json;
        }


        public static T DeserializeData<T>(string serializedSaveData) where T : class
        {
            if (serializedSaveData == null || serializedSaveData.IsEmpty())
            {
                return null;
            }
            var result = JsonUtility.FromJson<T>(serializedSaveData);
            UnityEngine.Debug.Log(serializedSaveData);
            return result;
        }
    }
}