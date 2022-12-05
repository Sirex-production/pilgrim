using Leopotam.Ecs;

namespace Ingame.Health
{
    public sealed class StopBleedingSystem : IEcsRunSystem
    {
        private readonly EcsFilter<HealthComponent, StopBleedingTag>.Exclude<DeathTag> _bleedingFilter;

        public void Run()
        {
            foreach (var i in _bleedingFilter)
            {
                ref var bleedingEntity = ref _bleedingFilter.GetEntity(i);
                
                bleedingEntity.Del<StopBleedingTag>();
                
                if(bleedingEntity.Has<BleedingComponent>())
                    bleedingEntity.Del<BleedingComponent>();
            }
        }
    }
}