using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame.SaveLoad
{
    [Serializable]
    public sealed class SaveLoadService
    {
        private const string TYPES_KEY = "Types";
        
        private ISerializer _serializer = new JsonSerializer();
        
        private bool _isNewTypeAdded = false;
        private SaveTypes _savedTypes;
        private Dictionary<Type, object> _persistentDataDictionary = new();
        private Dictionary<Type, bool> _cachedDataDictionary = new();
        
        public void Initialize()
        {
            LoadData();
        }
        
        private void LoadData()
        {
            string serializedSavedTypes = PlayerPrefs.GetString(TYPES_KEY, null);
            
            _savedTypes = _serializer.DeserializeData<SaveTypes>(serializedSavedTypes);
            _savedTypes ??= new();
            
            if (_savedTypes.storedTypes == null || _savedTypes.storedTypes.Count < 1)
            {
                _savedTypes.savedTypes ??= new List<Type>();
                _savedTypes.storedTypes ??= new List<string>();
                
                return;
            }
            _savedTypes.savedTypes ??= new List<Type>();
            foreach (var i in _savedTypes.storedTypes)
            {
                Type type = Type.GetType(i);
                _savedTypes.savedTypes.Add(type);
            }
            
        
            foreach (var type in _savedTypes.savedTypes)
            {
                var rawData = PlayerPrefs.GetString($"{type.ToString()}");
                var loadedData = _serializer.DynamicDeserializeTo(rawData, type);
                _persistentDataDictionary.Add(type, loadedData);
                _cachedDataDictionary.Add(type, false);
            }
        }

        public void Save()
        {
            if (_isNewTypeAdded)
            {
                var serializedSavedTypes = _serializer.SerializeData(_savedTypes);
                PlayerPrefs.SetString(TYPES_KEY, serializedSavedTypes);
            }

            _isNewTypeAdded = false;
            
            foreach (var keyValue in _persistentDataDictionary)
            {
                bool isDataModified = _cachedDataDictionary[keyValue.Key];
                
                if(!isDataModified)
                    continue;
                
                string serializedData = _serializer.SerializeData(keyValue.Value);
                PlayerPrefs.SetString($"{keyValue.Value}", serializedData);
            }
        }

        public bool HasData<T>() where T : class, new()
        {
            return _persistentDataDictionary.ContainsKey(typeof(T));
        }

        public void SetData<T>(T data) where T : class, new()
        {
            var type = typeof(T);

            if (HasData<T>())
            {
                _persistentDataDictionary[type] = data;
                _cachedDataDictionary[type] = true;
                
                return;
            }

            _persistentDataDictionary.Add(type, data);
            _cachedDataDictionary.Add(type, true);
            _savedTypes.savedTypes.Add(typeof(T));
            _savedTypes.storedTypes.Add(typeof(T).ToString());
            _isNewTypeAdded = true;
        }
        
        public T GetData<T>() where T : class, new()
        {
            if(!HasData<T>())
            {
                var data = new T();
                SetData(data);
                
                return data;
            }

            return (T)_persistentDataDictionary[typeof(T)];
        }
    }

    [Serializable]
    public class SaveTypes
    {
        [NonSerialized]
        public List<Type> savedTypes;
        public List<string> storedTypes;
    }
}