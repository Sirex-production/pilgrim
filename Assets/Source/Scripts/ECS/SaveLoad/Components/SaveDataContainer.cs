using System;
using Ingame.Inventory;
using JetBrains.Annotations;
using Newtonsoft.Json.Serialization;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ingame.SaveLoad
{
    [Serializable]
    public sealed class SaveDataContainer
    {
        public const string LEVEL_PERSISTANCE_NAME = "level_save";
        public const string PLAYER_PERSISTANCE_NAME = "player_save";
        
        private DataPersistenceWrapper<LevelPersistenceData> levelPersistence;
        private DataPersistenceWrapper<PlayerData> playerPersistence;

        public DataPersistenceWrapper<LevelPersistenceData> LevelPersistence => levelPersistence;

        public DataPersistenceWrapper<PlayerData> PlayerPersistence => playerPersistence;

        public void Init()
        {
            levelPersistence = TryToLoadData<LevelPersistenceData>(LEVEL_PERSISTANCE_NAME);
            playerPersistence = TryToLoadData<PlayerData>(PLAYER_PERSISTANCE_NAME);
        }

        private DataPersistenceWrapper<T> TryToLoadData<T>(string name) where T : class, new()
        {
            var encData = PlayerPrefs.GetString(name, null);
            if (encData == null)
            {
                return new DataPersistenceWrapper<T>(false,new T());
            }

            var value = BinarySerializer.DeserializeData<T>(encData);
            return value == null ? new DataPersistenceWrapper<T>(false,new T()) : new DataPersistenceWrapper<T>(true,value);
        }
    }

    [Serializable]
    public sealed class PlayerData
    {
        private float health;
        private AmmoBoxPersistenceData ammo;
        private InventoryPersistenceData inventory;
        private WeaponId? firstWeapon;
        private WeaponId? secondWeapon;

        public float Health
        {
            get => health;
            set
            {
                if (value <=1)
                {
                    return;
                }
                health = value;
            }
        }

        public AmmoBoxPersistenceData Ammo
        {
            get => ammo;
            set => ammo = value;
        }

        public InventoryPersistenceData Inventory
        {
            get => inventory;
            set
            {
                if(value == null)
                    return;
                inventory = value;
            }
        }

        [CanBeNull]
        public WeaponId? FirstWeapon
        {
            get => firstWeapon;
            set => firstWeapon = value;
        }

        [CanBeNull]
        public WeaponId? SecondWeapon
        {
            get => secondWeapon;
            set => secondWeapon = value;
        }
    }

    [Serializable]
    public sealed class WeaponId
    {
        public int type;
        public int variation;
    }
    
    [Serializable]
    public sealed class LevelPersistenceData
    {
        private int _level;
        public int Level => _level;
        
        public LevelPersistenceData() { }
        public LevelPersistenceData(int level)
        {
            _level = level;
        }

    }

    public sealed class InventoryPersistenceData
    {
        private int _currentNumberOfEnergyDrinks;
        private int _currentNumberOfCreamTubes;
        private int _currentNumberOfInhalators;
        private int _currentNumberOfMorphine;
        private int _currentNumberOfAdrenaline;
        private int _currentNumberOfBandages;

        public int CurrentNumberOfEnergyDrinks => _currentNumberOfEnergyDrinks;

        public int CurrentNumberOfCreamTubes => _currentNumberOfCreamTubes;

        public int CurrentNumberOfInhalators => _currentNumberOfInhalators;

        public int CurrentNumberOfMorphine => _currentNumberOfMorphine;

        public int CurrentNumberOfAdrenaline => _currentNumberOfAdrenaline;

        public int CurrentNumberOfBandages => _currentNumberOfBandages;

        public InventoryPersistenceData() { }

        public InventoryPersistenceData(int currentNumberOfEnergyDrinks, int currentNumberOfCreamTubes, int currentNumberOfInhalators, int currentNumberOfMorphine, int currentNumberOfAdrenaline, int currentNumberOfBandages)
        {
            _currentNumberOfEnergyDrinks = currentNumberOfEnergyDrinks;
            _currentNumberOfCreamTubes = currentNumberOfCreamTubes;
            _currentNumberOfInhalators = currentNumberOfInhalators;
            _currentNumberOfMorphine = currentNumberOfMorphine;
            _currentNumberOfAdrenaline = currentNumberOfAdrenaline;
            _currentNumberOfBandages = currentNumberOfBandages;
        }
    }

    public sealed class AmmoBoxPersistenceData
    {
        private int[] _ammo;

        public int[] Ammo => _ammo;
        
        public AmmoBoxPersistenceData() { }
        public AmmoBoxPersistenceData(int[] ammo)
        {
            _ammo = ammo;
        }

      
    }
    
    public class DataPersistenceWrapper<T> where T : new()
    {
        private bool _wasModified;
        private T _value;

        public DataPersistenceWrapper(bool wasModified, T value)
        {
            _wasModified = wasModified;
            _value = value;
        }

        public bool WasModified => _wasModified;
        public T Value
        {
            get => _value;
            set
            {
                _wasModified = true;
                _value = value;
            }
        }
    }
}