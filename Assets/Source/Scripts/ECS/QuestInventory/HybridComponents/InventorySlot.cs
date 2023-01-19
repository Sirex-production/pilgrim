using System;
using NaughtyAttributes;
using UnityEngine;

namespace Ingame.QuestInventory
{
    [Serializable]
    public sealed class InventorySlot : MonoBehaviour
    {
        [SerializeField]
        [Required] 
        private PickableItemConfig itemConfig;

        public PickableItemConfig ItemConfig => itemConfig;
    }
}