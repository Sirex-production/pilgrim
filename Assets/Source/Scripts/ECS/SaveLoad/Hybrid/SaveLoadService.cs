using System;
using UnityEngine;
using Zenject;

namespace Ingame.SaveLoad
{
    [Serializable]
    public sealed class SaveLoadService : IInitializable
    {
        private const string DATA_PERSISTANCE_NAME = "SaveData";
        
        private ISerializer _serializer = new JsonSerializer();
        private PersistenceDataContainer _persistenceDataContainer;

        public PersistenceDataContainer PersistenceDataContainer => _persistenceDataContainer;

        public void Initialize()
        {
            _persistenceDataContainer = TryToLoadData();
        }
        
        private PersistenceDataContainer TryToLoadData()
        {
            var encData = PlayerPrefs.GetString(DATA_PERSISTANCE_NAME, null);
            if (encData == null)
            {
                var result = new PersistenceDataContainer()
                {
                    LevelPersistence = new(false, new ()),
                   
                };
                
                return result;
            }

            var value = _serializer.DeserializeData<PersistenceDataContainer>(encData);
            return value ?? new PersistenceDataContainer();
        }

        public void SaveData()
        {
            var serializedData = _serializer.SerializeData(_persistenceDataContainer);
            PlayerPrefs.SetString(DATA_PERSISTANCE_NAME, serializedData);
        }
    }

    [Serializable]
    public sealed class PersistenceDataContainer
    {
        private DataPersistenceWrapper<LevelPersistenceData> levelPersistence = new();

        public DataPersistenceWrapper<LevelPersistenceData> LevelPersistence
        {
            get => levelPersistence;
            set => levelPersistence = value;
        }
    }

    [Serializable]
    public class DataPersistenceWrapper<T> where T : new()
    {
        private bool wasModified;
        private T value;

        public DataPersistenceWrapper()
        {
            wasModified = false;
        }
        
        public DataPersistenceWrapper(bool wasModified, T value)
        {
            this.wasModified = wasModified;
            this.value = value;
        }

        public bool WasModified => wasModified;
        
        public T Value
        {
            get => value;
            set
            {
                wasModified = true;
                this.value = value;
            }
        }
    }
    
    [Serializable]
    public sealed class LevelPersistenceData
    {
        [SerializeField]
        private int level;
        
        public int Level => level;
        
        public LevelPersistenceData() { }
        public LevelPersistenceData(int level)
        {
            this.level = level;
        }
    }
}