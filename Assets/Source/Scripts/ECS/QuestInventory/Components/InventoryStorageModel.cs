using System.Collections.Generic;
using UnityEngine;

namespace Ingame.QuestInventory
{
    public struct InventoryStorageModel
    {
        public Dictionary<PickableItemConfig, List<Transform>> slots;
    }
}