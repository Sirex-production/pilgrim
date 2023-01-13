using System;
using UnityEngine;

namespace Ingame.QuestInventory
{
       
    [Serializable]
    [CreateAssetMenu(menuName = "Ingame/Quest/Item", fileName = "Item")]
    public sealed class PickableItemConfig : ScriptableObject
    {
        [SerializeField] 
        private string itemName ;
        
        public string ItemName => itemName;
 
    }
}