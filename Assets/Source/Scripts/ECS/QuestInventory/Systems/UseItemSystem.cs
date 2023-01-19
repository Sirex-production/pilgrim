using System.Runtime.CompilerServices;
using Ingame.Interaction.Common;
using Ingame.Movement;
using Leopotam.Ecs;
using UnityEngine;


namespace Ingame.QuestInventory 
{
    public sealed class UseItemSystem : IEcsRunSystem
    {
        private readonly EcsFilter<PerformInteractionTag, TransformModel, ItemModel, PickedUpItemTag> _useItemFilter;
        private readonly EcsFilter<InventoryStorageModel> _backpackFilter;
        
        public void Run()
        {
            ProcessUsageOfItem();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ProcessUsageOfItem()
        {
            foreach (var i in _useItemFilter)
            {
                ref var entity = ref _useItemFilter.GetEntity(i);
                ref var transformModel = ref _useItemFilter.Get2(i);

                if (transformModel.transform.TryGetComponent(out UsableItem usableItem))
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
    }
}