using System;
using Ingame.Health;
using Ingame.Player;
using Leopotam.Ecs;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Ingame.Effects
{
    public sealed class GasChokeDisplaySystem : IEcsRunSystem
    {
        private readonly EcsFilter<PlayerModel, HealthComponent> _playerHealthFilter;
        private readonly EcsFilter<PostProcessingLensDistortionModel, GasChokeEffectComponent> _gasChokeEffectFilter;

        public void Run()
        {
            if(_playerHealthFilter.IsEmpty())
                return;

            ref var playerEntity = ref _playerHealthFilter.GetEntity(0);
            bool isPlayerUnderGasChoke = playerEntity.Has<GasChokeComponent>();
            
            foreach (var i in _gasChokeEffectFilter)
            {
                ref var lensDistortionModel = ref _gasChokeEffectFilter.Get1(i);
                ref var gasChokeEffectComp = ref _gasChokeEffectFilter.Get2(i);
                ref var lensDistortion = ref lensDistortionModel.lensDistortion;
                float lerpingT = gasChokeEffectComp.effectSpeed * Time.deltaTime;

                if (isPlayerUnderGasChoke)
                {
                    if (Math.Abs(gasChokeEffectComp.targetIntensity - lensDistortion.intensity.value) < .01f &&
                        (gasChokeEffectComp.targetCenter - lensDistortion.center.value).sqrMagnitude < .01f)
                    {
                        gasChokeEffectComp.targetIntensity = Random.Range(gasChokeEffectComp.minIntensity, gasChokeEffectComp.maxIntensity);
                        gasChokeEffectComp.targetIntensity *= Random.value > .05f ? 1 : -1;

                        gasChokeEffectComp.targetCenter = new Vector2
                        {
                            x = Random.Range(-gasChokeEffectComp.maxCenterOffset, gasChokeEffectComp.maxCenterOffset),
                            y = Random.Range(-gasChokeEffectComp.maxCenterOffset, gasChokeEffectComp.maxCenterOffset)
                        };
                        gasChokeEffectComp.targetCenter += lensDistortionModel.initialCenter;
                    }
                    
                    lensDistortion.intensity.value = Mathf.LerpUnclamped(lensDistortion.intensity.value, gasChokeEffectComp.targetIntensity, lerpingT);
                    lensDistortion.center.value = Vector2.LerpUnclamped(lensDistortion.center.value, gasChokeEffectComp.targetCenter, lerpingT);
                    
                    continue;
                }

                lensDistortion.intensity.value = Mathf.Lerp(lensDistortion.intensity.value, lensDistortionModel.initialIntensity, lerpingT);
                lensDistortion.center.value = Vector2.Lerp(lensDistortion.center.value, lensDistortionModel.initialCenter, lerpingT);
            }
        }
    }
}