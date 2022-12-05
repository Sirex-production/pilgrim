using Ingame.Health;
using Ingame.Player;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Effects
{
    public sealed class EnergyEffectDisplaySystem : IEcsRunSystem
    {
        private readonly EcsFilter<PlayerModel, HealthComponent> _playerHealthFilter;
        private readonly EcsFilter<PostProcessingChromaticAberrationModel, EnergyDisplayEffectComponent> _energyDisplayEffectFilter;

        public void Run()
        {
            if(_playerHealthFilter.IsEmpty())
                return;

            ref var playerEntity = ref _playerHealthFilter.GetEntity(0);
            bool isPlayerUnderEnergyEffect = playerEntity.Has<EnergyEffectComponent>();
            
            foreach (var i in _energyDisplayEffectFilter)
            {
                ref var chromaticAberrationModel = ref _energyDisplayEffectFilter.Get1(i);
                ref var energyDisplayEffectComponent = ref _energyDisplayEffectFilter.Get2(i);
                ref var chromaticAberration = ref chromaticAberrationModel.chromaticAberration;
                var lerpT = energyDisplayEffectComponent.effectSpeed * Time.deltaTime;
                
                if (isPlayerUnderEnergyEffect)
                {
                    ref var energyEffect = ref playerEntity.Get<EnergyEffectComponent>();
                    float targetIntensity = chromaticAberrationModel.initialIntensity;
                    targetIntensity += Mathf.InverseLerp(0, EnergyEffectComponent.NUMBER_OF_ENERGY_EFFECTS_TO_DIE, energyEffect.numberOfEffects);

                    chromaticAberration.intensity.value = Mathf.Lerp(chromaticAberration.intensity.value, targetIntensity, lerpT);
                    
                    continue;
                }

                chromaticAberration.intensity.value = Mathf.Lerp(chromaticAberration.intensity.value, chromaticAberrationModel.initialIntensity, lerpT);
            }
        }
    }
}