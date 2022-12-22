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
            Entity.Get<EnemyStateModel>().isTakingDamage = false;
            ref var health = ref Entity.Get<HealthComponent>();
            health.initialHealth = health.currentHealth;
            
            return State.Success;
        }
    }
}