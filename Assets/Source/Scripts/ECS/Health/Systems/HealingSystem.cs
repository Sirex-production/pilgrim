using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Health
{
    public sealed class HealingSystem : IEcsRunSystem
    {
        private readonly EcsFilter<HealthComponent, HealComponent>.Exclude<DeathTag> _healFilter;

        public void Run()
        {
            foreach (var i in _healFilter)
            {
                ref var healEntity = ref _healFilter.GetEntity(i);
                ref var healthComp = ref _healFilter.Get1(i);
                ref var healComp = ref _healFilter.Get2(i);

                healComp.hpToRestore = Mathf.Max(0, healComp.hpToRestore);
                healthComp.currentHealth = Mathf.Min(healthComp.initialHealth, healthComp.currentHealth + healComp.hpToRestore);
                
                healEntity.Del<HealComponent>();
            }
        }
    }
}