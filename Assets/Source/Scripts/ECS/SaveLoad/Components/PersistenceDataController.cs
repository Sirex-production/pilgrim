using System;
using Ingame.Inventory;
using JetBrains.Annotations;
using Newtonsoft.Json.Serialization;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ingame.SaveLoad
{
    
    
    [Serializable]
    public sealed class PersistenceDataController
    {
        public const string DATA_PERSISTANCE_NAME = "SaveData";
        public PersistenceDataContainer persistenceDataContainer;
      
        public void Init()
        {
            persistenceDataContainer = TryToLoadData(DATA_PERSISTANCE_NAME);
        }
        
        private PersistenceDataContainer TryToLoadData(string name)
        {
            var encData = PlayerPrefs.GetString(name, null);
            if (encData == null)
            {
                var result = new PersistenceDataContainer()
                {
                    LevelPersistence = new(false, new ()),
                   
                };
                return result;
            }

            var value = JsonSerializer.DeserializeData<PersistenceDataContainer>(encData);
            return value ?? new PersistenceDataContainer()
            {
                LevelPersistence = new(false, new ()),
             
            };
        }
    }

    [Serializable]
    public sealed class PersistenceDataContainer
    {
        [SerializeField]
        private DataPersistenceWrapper<LevelPersistenceData> levelPersistence;
   

        public DataPersistenceWrapper<LevelPersistenceData> LevelPersistence
        {
            get => levelPersistence;
            set => levelPersistence = value;
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
      
    
    [Serializable]
    public class DataPersistenceWrapper<T> where T : new()
    {
        [SerializeField]
        private bool wasModified;
        [SerializeField]
        private T value;

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
}