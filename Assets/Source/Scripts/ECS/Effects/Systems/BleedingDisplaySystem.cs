using System;
using Ingame.Health;
using Ingame.Player;
using Ingame.UI;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Effects
{
    public sealed class BleedingDisplaySystem : IEcsRunSystem
    {
        private readonly EcsFilter<PlayerModel, HealthComponent> _playerHealthFilter;
        private readonly EcsFilter<CanvasGroupModel, UiBleedingCanvasGroupComponent> _uiBleedingDisplayFilter;

        public void Run()
        {
            if (_playerHealthFilter.IsEmpty())
                return;

            ref var playerEntity = ref _playerHealthFilter.GetEntity(0);
            bool isPlayerBleeding = playerEntity.Has<BleedingComponent>();
            
            foreach (var i in _uiBleedingDisplayFilter)
            {
                ref var canvasGroup = ref _uiBleedingDisplayFilter.Get1(i).canvasGroup;
                ref var uiBleedingCanvasGroupComp = ref _uiBleedingDisplayFilter.Get2(i);
                var lerpingT = uiBleedingCanvasGroupComp.fadingSpeed * Time.deltaTime;

                if (isPlayerBleeding)
                {
                    float minAlpha = uiBleedingCanvasGroupComp.minimumAlphaDuringBleeding;
                    float maxAlpha = uiBleedingCanvasGroupComp.maximumAlphaDuringBleeding;
                    
                    if (Math.Abs(canvasGroup.alpha - maxAlpha) < .001f)
                        uiBleedingCanvasGroupComp.isLerpingTowardsPositiveValue = false;
                    else if (Math.Abs(canvasGroup.alpha - minAlpha) < .001f)
                        uiBleedingCanvasGroupComp.isLerpingTowardsPositiveValue = true;
                    
                    float targetValue = uiBleedingCanvasGroupComp.isLerpingTowardsPositiveValue ? maxAlpha : minAlpha;

                    canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetValue, lerpingT);
                    
                    continue;
                }
                
                canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0, lerpingT);
            }
        }
    }
}