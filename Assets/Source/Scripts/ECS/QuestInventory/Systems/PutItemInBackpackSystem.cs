using System.Runtime.CompilerServices;
using Ingame.Interaction.Common;
using Ingame.Movement;
using Leopotam.Ecs;

namespace Ingame.QuestInventory
{
    public sealed class PutItemInBackpackSystem : IEcsRunSystem
    {
        private readonly EcsFilter<PerformInteractionTag, TransformModel,ItemModel>.Exclude<PickedUpItemTag> _pickItemFilter;
        private readonly EcsFilter<InventoryStorageModel> _backpackFilter;
        
        public void Run()
        {
            ProcessPickUpItem();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
                       || entityReference.Entity.Has<OccupiedInventorySlotTag>())
                        continue;
                    
                    entityReference.Entity.Get<OccupiedInventorySlotTag>();
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