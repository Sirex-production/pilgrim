using Leopotam.Ecs;

namespace Ingame.Health
{
    public class DamageSystem : IEcsRunSystem
    {
        private readonly EcsFilter<DamageComponent, HealthComponent>.Exclude<DeathTag> _healthDamageFilter;

        public void Run()
        {
            foreach (var i in _healthDamageFilter)
            {
                ref var entity = ref _healthDamageFilter.GetEntity(i);
                ref var damageReq = ref _healthDamageFilter.Get1(i);
                ref var healthComp = ref _healthDamageFilter.Get2(i);

                if(damageReq.damageToDeal > 0)
                    healthComp.currentHealth -= damageReq.damageToDeal;
                
                entity.Del<DamageComponent>();
            }
        }
    }
}