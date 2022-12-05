using Ingame.Player;
using Leopotam.Ecs;
using Support.Extensions;
using UnityEngine;

namespace Ingame.Inventory
{
    public sealed class UpdateBackpackItemsAppearanceSystem : IEcsRunSystem, IEcsInitSystem
    {
        private readonly EcsWorld _world;
        private readonly EcsFilter<PlayerModel, InventoryComponent> _playerInventoryFilter;
        private readonly EcsFilter<BackpackModel> _backpackFilter;
        private readonly EcsFilter<UpdateBackpackAppearanceEvent> _updateInventoryEventFilter;

        public void Init()
        {
            _world.NewEntity().Get<UpdateBackpackAppearanceEvent>();
        }
        
        public void Run()
        {
            if (_updateInventoryEventFilter.IsEmpty() || _backpackFilter.IsEmpty() || _playerInventoryFilter.IsEmpty())
                return;
            
            ref var backpackModel = ref _backpackFilter.Get1(0);
            ref var playerInventoryComp = ref _playerInventoryFilter.Get2(0);

            int maxAmountOfTransforms = Mathf.Max
            (
                backpackModel.MaxAmountOfAdrenaline,
                backpackModel.MaxAmountOfBandages,
                backpackModel.MaxAmountOfCream,
                backpackModel.MaxAmountOfInhalators,
                backpackModel.MaxAmountOfMorphine,
                backpackModel.MaxAmountOfEnergyDrinks
            );

            int adrenalineLeftToFill = playerInventoryComp.currentNumberOfAdrenaline;
            int bandagesLeftToFill = playerInventoryComp.currentNumberOfBandages;
            int creamTubesLeftToFill = playerInventoryComp.currentNumberOfCreamTubes;
            int inhalatorsLeftToFill = playerInventoryComp.currentNumberOfInhalators;
            int morphineLeftToFill = playerInventoryComp.currentNumberOfMorphine;
            int energyDrinksLeftToFill = playerInventoryComp.currentNumberOfEnergyDrinks;

            for (var i = 0; i < maxAmountOfTransforms; i++)
            {
                if (i < backpackModel.MaxAmountOfAdrenaline)
                    UpdateItemAppearance(i, ref adrenalineLeftToFill, backpackModel.adrenalineInsideBackpack);
                
                if (i < backpackModel.MaxAmountOfBandages)
                    UpdateItemAppearance(i, ref bandagesLeftToFill, backpackModel.bandagesInsideBackpack);
                
                if (i < backpackModel.MaxAmountOfCream)
                    UpdateItemAppearance(i, ref creamTubesLeftToFill, backpackModel.creamTubesInsideBackpack);
                
                if (i < backpackModel.MaxAmountOfEnergyDrinks)
                    UpdateItemAppearance(i, ref energyDrinksLeftToFill, backpackModel.energyDrinksInsideBackpack);
                
                if (i < backpackModel.MaxAmountOfInhalators)
                    UpdateItemAppearance(i, ref inhalatorsLeftToFill, backpackModel.inhalatorsInsideBackpack);
                
                if (i < backpackModel.MaxAmountOfMorphine)
                    UpdateItemAppearance(i, ref morphineLeftToFill, backpackModel.morphineInsideBackpack);
            }
            
            void UpdateItemAppearance(int transformIndex, ref int itemsLeftToRefill, Transform[] itemsTransforms)
            {
                var backpackItemTransform = itemsTransforms[transformIndex];
                    
                if (itemsLeftToRefill > 0)
                {
                    itemsLeftToRefill--;

                    if (!backpackItemTransform.IsGameObjectActive())
                        backpackItemTransform.SetGameObjectActive();
                }
                else
                {
                    backpackItemTransform.SetGameObjectInactive();
                }
            }
        }
    }
}