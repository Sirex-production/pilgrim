using System;
using Ingame.Inventory;
using Newtonsoft.Json.Serialization;
using UnityEngine;

namespace Ingame.SaveLoad
{
    [Serializable]
    public sealed class SaveDataContainer
    {
        public LevelData level;
        public PlayerData player;
    }

    [Serializable]
    public sealed class  PlayerData
    {
        public float health;
        public AmmoBoxComponent ammo;
        public InventoryComponent inventory;
        public WeaponId? firstWeapon;
        public WeaponId? secondWeapon;

    }

    [Serializable]
    public sealed class  WeaponId
    {
        public int type;
        public int variation;
    }
    
    [Serializable]
    public sealed class  LevelData
    {
        public int level;
    }
}