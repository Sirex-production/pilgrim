using Ingame.Player;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Health
{
    public sealed class ManageEnergyEffectSystem : IEcsRunSystem
    {
        private readonly EcsFilter<PlayerModel, EnergyEffectComponent> _playerUnderEnergyDrinkFilter;

        public void Run()
        {
            foreach (var i in _playerUnderEnergyDrinkFilter)
            {
                ref var playerEntity = ref _playerUnderEnergyDrinkFilter.GetEntity(i);
                ref var energyEffectComp = ref _playerUnderEnergyDrinkFilter.Get2(i);

                energyEffectComp.duration -= Time.deltaTime;
                if (energyEffectComp.duration < 0) 
                    playerEntity.Del<EnergyEffectComponent>();

                if (energyEffectComp.numberOfEffects > EnergyEffectComponent.NUMBER_OF_ENERGY_EFFECTS_TO_DIE)
                {
                    playerEntity.Del<EnergyEffectComponent>();
                    playerEntity.Get<DeathTag>();
                }
            }
        }
    }
}