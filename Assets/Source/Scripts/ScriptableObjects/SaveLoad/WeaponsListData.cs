using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Ingame.SaveLoad
{
    [Serializable]
    [CreateAssetMenu(menuName = "Ingame/SaveLoad/WeaponsList", fileName = "WeaponsList")]
    public sealed class WeaponsListData : ScriptableObject
    {
        [SerializeField] private List<WeaponTypeDataHolder> weaponTypeDataHolders;
    }
    
    [Serializable]
    public class WeaponTypeDataHolder
    {
        [SerializeField] 
        private string name;
        
        [SerializeField] 
        private int id;
        
        [SerializeField]
        private List<WeaponVariationDatHolder> weaponVariationDatHolders;
        
        public List<WeaponVariationDatHolder> WeaponVariationDatHolders => weaponVariationDatHolders;
        public int ID => id;
    }
    
    [Serializable]
    public class WeaponVariationDatHolder
    {
        [SerializeField] 
        [Required]
        private GameObject weapon;
        
        [SerializeField] 
        private int id;

        public GameObject Weapon => weapon;

        public int ID => id;
    }
}