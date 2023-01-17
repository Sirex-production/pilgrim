using System.Runtime.CompilerServices;
using Ingame.Extensions;
using Ingame.Interaction.Common;
using Ingame.Movement;
using Leopotam.Ecs;
using Support.Extensions;
using UnityEngine;

namespace Ingame.QuestInventory
{
    public sealed class PutItemInBackpackSystem : IEcsRunSystem
    {
        private static readonly int HUD_LAYER = LayerMask.NameToLayer("HUD");
        
        private readonly EcsWorld _world;
        private readonly EcsFilter<PerformInteractionTag, TransformModel, ItemModel>.Exclude<PickedUpItemTag> _pickItemFilter;
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
                
                ref var itemEntity = ref _pickItemFilter.GetEntity(i);
                ref var itemTransformModel = ref _pickItemFilter.Get2(i);
                ref var itemModel = ref _pickItemFilter.Get3(i);

                if (!backpack.slots.ContainsKey(itemModel.itemConfig))
                {
#if UNITY_EDITOR
                    UnityEngine.Debug.LogWarning($"{backpack} does not have a slot that corresponds to the item :{itemModel.itemConfig}");
#endif
                    itemEntity.Del<PerformInteractionTag>();
                    continue;
                }

                var slots = backpack.slots[itemModel.itemConfig];
                foreach (var slotTransform in slots)
                {
                    if(!slotTransform.TryGetComponent(out EntityReference slotEntityReference) 
                       || slotEntityReference.Entity.Has<OccupiedInventorySlotTag>())
                        continue;
                    
                    slotEntityReference.Entity.Get<OccupiedInventorySlotTag>();
                    
                    itemTransformModel.transform.SetParent(slotTransform);
                    itemTransformModel.transform.localPosition = Vector3.zero;
                    itemTransformModel.transform.localEulerAngles = Vector3.zero;
                    itemTransformModel.transform.gameObject.SetLayerToAllChildrenAndSelf(HUD_LAYER);
                    
                    itemEntity.Get<PickedUpItemTag>();
                    itemEntity.Del<PerformInteractionTag>();
                    
                    if(!itemEntity.Has<ConsumableItemTag>() && !itemTransformModel.transform.TryGetComponent(out UsableItem _))
                        itemEntity.Del<InteractiveTag>();

                    _world.SendSignal<InventoryIsUpdatedEvent>();
                }
                
                itemEntity.Del<PerformInteractionTag>();
            }
        }
    }
}