using Ingame.Behaviour;
using Ingame.Health;
using Leopotam.Ecs;

namespace Ingame.Enemy
{
    public sealed class NullifyTakenDamageActionNode : ActionNode
    {
        protected override void ActOnStart()
        {
             
        }

        protected override void ActOnStop()
        {
             
        }

        protected override State ActOnTick()
        {
            entity.Get<EnemyStateModel>().isTakingDamage = false;
            ref var health = ref entity.Get<HealthComponent>();
            health.initialHealth = health.currentHealth;
            
            return State.Success;
        }
    }
}