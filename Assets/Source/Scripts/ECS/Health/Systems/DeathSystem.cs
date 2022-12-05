using Leopotam.Ecs;

namespace Ingame.Health
{
    public class DeathSystem : IEcsRunSystem
    {
        private readonly EcsFilter<HealthComponent>.Exclude<DeathTag> _healthFilter;

        public void Run()
        {
            foreach (var i in _healthFilter)
            {
                ref var healthEntity = ref _healthFilter.GetEntity(i); 
                ref var healthComp = ref _healthFilter.Get1(i);

                if (healthComp.currentHealth <= 1)
                    healthEntity.Get<DeathTag>();
            }
        }
    }
}