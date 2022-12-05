using Leopotam.Ecs;

namespace Ingame.Health
{
    public sealed class StopGasChokeSystem : IEcsRunSystem 
    {
        private readonly EcsFilter<HealthComponent, StopGasChokeTag>.Exclude<DeathTag> _gasChokeFilter;

        public void Run()
        {
            foreach (var i in _gasChokeFilter)
            {
                ref var bleedingEntity = ref _gasChokeFilter.GetEntity(i);
                
                bleedingEntity.Del<StopGasChokeTag>();
                
                if(bleedingEntity.Has<GasChokeComponent>())
                    bleedingEntity.Del<GasChokeComponent>();
            }
        }
    }
}