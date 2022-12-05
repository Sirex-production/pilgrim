using Ingame.Health;
using Ingame.Interaction.Common;
using Ingame.Movement;
using Ingame.Player;
using Leopotam.Ecs;
using Support.Extensions;

namespace Ingame.Inventory
{
    public sealed class InteractWithBackpackItemSystem : IEcsRunSystem
    {
        private readonly EcsWorld _world;
        private readonly EcsFilter<TransformModel, BackpackItemTag, PerformInteractionTag> _backpackInteractItem;
        private readonly EcsFilter<PlayerModel, InventoryComponent, HealthComponent> _playerFilter;

        public void Run()
        {
            if(_playerFilter.IsEmpty())
                return;

            ref var playerEntity = ref _playerFilter.GetEntity(0);
            ref var playerHealthEntity = ref _playerFilter.GetEntity(0);
            ref var playerInventory = ref _playerFilter.Get2(0);

            _world.NewEntity().Get<UpdateBackpackAppearanceEvent>();

            foreach (var i in _backpackInteractItem)
            {
                ref var itemEntity = ref _backpackInteractItem.GetEntity(i);
                ref var itemTransformModel = ref _backpackInteractItem.Get1(i);
                
                itemEntity.Del<PerformInteractionTag>();
                itemTransformModel.transform.SetGameObjectInactive();

                if (itemEntity.Has<AdrenalineTag>())
                {
                    playerInventory.currentNumberOfAdrenaline--;
                }
                
                if (itemEntity.Has<BandageTag>())
                {
                    playerHealthEntity.Get<StopBleedingTag>();
                    playerInventory.currentNumberOfBandages--;
                }
                
                if (itemEntity.Has<CreamTag>())
                {
                    playerInventory.currentNumberOfCreamTubes--;
                }
                
                if (itemEntity.Has<EnergyDrinkTag>())
                {
                    playerInventory.currentNumberOfEnergyDrinks--;
                    ref var energyDrinkEffect = ref playerEntity.Get<EnergyEffectComponent>();

                    energyDrinkEffect.numberOfEffects++;
                    energyDrinkEffect.duration = EnergyEffectComponent.DEFAULT_EFFECT_DURATION;
                    energyDrinkEffect.movingSpeedScale = EnergyEffectComponent.DEFAULT_MOVING_SPEED_SCALE;
                }
                
                if (itemEntity.Has<InhalatorTag>())
                {
                    playerHealthEntity.Get<StopGasChokeTag>();
                    playerInventory.currentNumberOfInhalators--;
                }
                
                if (itemEntity.Has<MorphineTag>())
                {
                    playerHealthEntity.Get<HealComponent>().hpToRestore = MorphineTag.AMOUNT_OF_HP_RESTORED;
                    playerInventory.currentNumberOfMorphine--;
                }
            }
        }
    }
}