using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Voody.UniLeo;
using Zenject;

namespace Ingame.QuestInventory
{
    public sealed class InventoryStorageModelProvider : MonoProvider<InventoryStorageModel>
    {
        [Inject]
        private void Construct()
        {
            List<InventorySlot> slots = ModifyListOfAllItemsSlots(this.transform,  new());
            var dic = new Dictionary<PickableItemConfig, List<Transform>>();

            foreach (var slot in slots)
            {
                if (!dic.ContainsKey(slot.ItemConfig))
                {
                    dic.Add(slot.ItemConfig, new List<Transform>());
                }
                
                dic[slot.ItemConfig].Add(slot.transform);
            }
            
            
            value = new InventoryStorageModel()
            {
                slots = dic
            }; 
        }

        private List<InventorySlot> ModifyListOfAllItemsSlots(Transform trans,  List<InventorySlot> listOfSlots)
        {
            foreach (Transform child in trans)
            {
                if (!child.TryGetComponent<InventorySlot>(out var slot)) 
                    continue;

                listOfSlots.Add(slot);
                ModifyListOfAllItemsSlots(child,  listOfSlots);
            }

            return listOfSlots;
        }
    }
}