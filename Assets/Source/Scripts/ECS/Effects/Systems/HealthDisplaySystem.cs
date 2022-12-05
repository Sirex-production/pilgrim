using Ingame.Health;
using Ingame.Player;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Effects
{
    public sealed class HealthDisplaySystem : IEcsRunSystem, IEcsDestroySystem
    {
        private const float EFFECTS_LERP_SPEED = 4f;
        
        private readonly EcsFilter<Vignette2DMaterialModel> _vignetteFilter;
        private readonly EcsFilter<PlayerModel, HealthComponent> _playerHealthFilter;

        public void Run()
        {
            if (_playerHealthFilter.IsEmpty() || _vignetteFilter.IsEmpty())
                return;
            
            ref var playerHealth = ref _playerHealthFilter.Get2(0);
            
            foreach (var i in _vignetteFilter)
            {
                ref var vignetteComp = ref _vignetteFilter.Get1(i);
                var vignette2DMaterial = vignetteComp.vignette2DMaterial;
                float currentVignetteValue = vignette2DMaterial.GetFloat(vignetteComp.RADIUS_PROP_ID);
                float targetVignetteValue = 1 - Mathf.InverseLerp(0, playerHealth.initialHealth, playerHealth.currentHealth);
                float lerpVignetteValue = Mathf.Lerp(currentVignetteValue, targetVignetteValue, EFFECTS_LERP_SPEED * Time.deltaTime);
                
                vignette2DMaterial.SetFloat(vignetteComp.RADIUS_PROP_ID, lerpVignetteValue);
            }
        }

        public void Destroy()
        {
            foreach (var i in _vignetteFilter)
            {
                ref var vignetteComp = ref _vignetteFilter.Get1(i);
                var vignette2DMaterial = vignetteComp.vignette2DMaterial;
                
                vignette2DMaterial.SetFloat(vignetteComp.RADIUS_PROP_ID, 0);
            }
        }
    }
}