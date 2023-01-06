using Ingame.Interaction.Common;
using Ingame.Movement;
using Leopotam.Ecs;
using UnityEngine;


namespace Ingame.QuestInventory 
{
    public sealed class ItemsSystem : IEcsRunSystem
    {
        private readonly EcsFilter<PerformInteractionTag, TransformModel,ItemModel>.Exclude<PickedUpItemTag> _pickItemFilter;
        private readonly EcsFilter<PerformInteractionTag, TransformModel,ItemModel,PickedUpItemTag> _useItemFilter;
        private readonly EcsFilter<InventoryStorageModel> _backpackFilter;
        
        public void Run()
        {
            ProcessPickUpItem();
            ProcessUsageOfItem();
        }
        
        private void ProcessUsageOfItem()
        {
            foreach (var i in _useItemFilter)
            {
                ref var entity = ref _useItemFilter.GetEntity(i);
                ref var transformModel = ref _useItemFilter.Get2(i);

                if (transformModel.transform.TryGetComponent<UsableItem>(out var usableItem))
                {
                    usableItem.Use();          
                }

                if (entity.Has<ConsumableItemTag>())
                {
                    Object.Destroy( transformModel.transform.gameObject);
                    entity.Destroy();
                    continue;
                }
                
                entity.Del<PerformInteractionTag>();
            }
        }
        
        private void ProcessPickUpItem()
        {
            foreach (var i in _pickItemFilter)
            {
                if (_backpackFilter.IsEmpty()) return;

                ref var backpack = ref _backpackFilter.Get1(0);
                
                ref var entity = ref _pickItemFilter.GetEntity(i);
                ref var transformModel = ref _pickItemFilter.Get2(i);
                ref var itemModel = ref _pickItemFilter.Get3(i);

                if (!backpack.slots.ContainsKey(itemModel.itemConfig))
                {
                    #if UNITY_EDITOR
                        UnityEngine.Debug.LogWarning($"{backpack} does not have a slot that corresponds to the item :{itemModel.itemConfig}");
                    #endif
                    entity.Del<PerformInteractionTag>();
                    continue;
                }

                var slots = backpack.slots[itemModel.itemConfig];
                foreach (var transform in slots)
                {
                    if(!transform.TryGetComponent<EntityReference>(out var entityReference) 
                       || entityReference.Entity.Has<OccupiedInventorySlot>())
                        continue;
                    
                    entityReference.Entity.Get<OccupiedInventorySlot>();
                    transformModel.transform.position = transform.position;
                    entity.Get<PickedUpItemTag>();
                    entity.Del<PerformInteractionTag>();
                    return;
                }
                
                entity.Del<PerformInteractionTag>();
            }
        }
    }
}