using System;
using NaughtyAttributes;

namespace Ingame.QuestInventory 
{
    [Serializable]
    public struct ItemModel
    {
        [Required]
        public PickableItemConfig itemConfig;
    }
}