using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Health
{
    public class BleedingSystem : IEcsRunSystem
    {
        private readonly EcsFilter<BleedingComponent, HealthComponent>.Exclude<DeathTag> _healthFilter;

        public void Run()
        {
            foreach (var i in _healthFilter)
            {
                ref var bleedingComp = ref _healthFilter.Get1(i);
                ref var healthComp = ref _healthFilter.Get2(i);

                bleedingComp.timePassedFromLastBloodLoss += Time.deltaTime;
                if (bleedingComp.timePassedFromLastBloodLoss < 1)
                    continue;
                
                bleedingComp.timePassedFromLastBloodLoss = 0;
                healthComp.currentHealth -= bleedingComp.healthTakenPerSecond;
            }
        }
    }
}