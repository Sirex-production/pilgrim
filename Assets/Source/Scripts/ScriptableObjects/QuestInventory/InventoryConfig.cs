using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.ObjectModel;
using UnityEditor;

namespace Ingame.QuestInventory
{
    [CreateAssetMenu(menuName = "Ingame/Inventory/Items", fileName = "Inventory")]
    public sealed class InventoryConfig : ScriptableObject
    {
        [SerializeField] private List<PickableItemConfig > items;
#if UNITY_EDITOR
        public event Action onValidateData;
#endif
        public ReadOnlyCollection<PickableItemConfig> Items => items.AsReadOnly();

#if UNITY_EDITOR
        private void OnValidate()
        {
            onValidateData?.Invoke();
        }
#endif
    }
}