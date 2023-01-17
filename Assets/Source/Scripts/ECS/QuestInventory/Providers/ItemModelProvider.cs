using NaughtyAttributes;
using UnityEngine;
using Voody.UniLeo;
using Zenject;

namespace Ingame.QuestInventory 
{
    public sealed class ItemModelProvider : MonoProvider<ItemModel>
    {
        [SerializeField]
        [Required]
        private PickableItemConfig pickableItemConfig;

        [Inject]
        private void Construct()
        {
            value = new ItemModel()
            {
                itemConfig = this.pickableItemConfig
            };
        }
    }
}