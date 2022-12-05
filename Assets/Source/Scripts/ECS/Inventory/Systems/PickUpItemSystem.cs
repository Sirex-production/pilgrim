using Ingame.Interaction.Common;
using Ingame.Movement;
using Ingame.Player;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Inventory
{
    public sealed class PickUpItemSystem : IEcsRunSystem
    {
        private readonly EcsWorld _world;
        private readonly EcsFilter<TransformModel, LootItemTag, PerformInteractionTag> _pickUpLootItemFilter;
        private readonly EcsFilter<PlayerModel, InventoryComponent> _playerInventoryFilter;
        private readonly EcsFilter<BackpackModel> _backpackFilter;

        public void Run()
        {
            if(_playerInventoryFilter.IsEmpty() || _backpackFilter.IsEmpty())
                return;

            ref var playerInventoryComp = ref _playerInventoryFilter.Get2(0);
            ref var backpackModel = ref _backpackFilter.Get1(0);
            
            foreach (var i in _pickUpLootItemFilter)
            {
                ref var lootItemEntity = ref _pickUpLootItemFilter.GetEntity(i);
                ref var lootItemTransformModel = ref _pickUpLootItemFilter.Get1(i);

                bool wasItemAdded =
                    TryAddItemToInventory<AdrenalineTag>(ref lootItemEntity, ref playerInventoryComp.currentNumberOfAdrenaline, backpackModel.MaxAmountOfAdrenaline) ||
                    TryAddItemToInventory<BandageTag>(ref lootItemEntity, ref playerInventoryComp.currentNumberOfBandages, backpackModel.MaxAmountOfBandages) ||
                    TryAddItemToInventory<CreamTag>(ref lootItemEntity, ref playerInventoryComp.currentNumberOfCreamTubes, backpackModel.MaxAmountOfCream) ||
                    TryAddItemToInventory<EnergyDrinkTag>(ref lootItemEntity, ref playerInventoryComp.currentNumberOfEnergyDrinks, backpackModel.MaxAmountOfEnergyDrinks) ||
                    TryAddItemToInventory<InhalatorTag>(ref lootItemEntity, ref playerInventoryComp.currentNumberOfInhalators, backpackModel.MaxAmountOfInhalators) ||
                    TryAddItemToInventory<MorphineTag>(ref lootItemEntity, ref playerInventoryComp.currentNumberOfMorphine, backpackModel.MaxAmountOfMorphine);

                if(wasItemAdded)
                {
                    _world.NewEntity().Get<UpdateBackpackAppearanceEvent>();
                    Object.Destroy(lootItemTransformModel.transform.gameObject);
                    lootItemEntity.Destroy();
                }
                
            }

            bool TryAddItemToInventory<T>(ref EcsEntity itemEntity, ref int currentNumberOfItems, in int maxAmountOfItems) where T : struct
            {
                if (!itemEntity.Has<T>())
                    return false;

                if (currentNumberOfItems + 1 <= maxAmountOfItems)
                {
                    currentNumberOfItems++;
                    return true;
                }

                itemEntity.Del<PerformInteractionTag>();
                return false;
            }
        }
    }
}