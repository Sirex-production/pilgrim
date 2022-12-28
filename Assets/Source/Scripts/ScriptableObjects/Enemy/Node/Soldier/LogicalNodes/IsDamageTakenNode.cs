using System;
using Ingame.Behaviour;
using Ingame.Health;
using Leopotam.Ecs;

namespace Ingame.Enemy
{
    public class IsDamageTakenNode : ActionNode
    {
        protected override void ActOnStart()
        {
             
        }

        protected override void ActOnStop()
        {
       
        }

        protected override State ActOnTick()
        {
            ref var enemyModel = ref Entity.Get<EnemyStateModel>();
            if (enemyModel.isTakingDamage)
            {
                return State.Success;
            }
            ref var healthComponent = ref Entity.Get<HealthComponent>();
            if (Math.Abs(healthComponent.initialHealth - healthComponent.currentHealth) < 0.1f) return State.Failure;
            
            enemyModel.isTakingDamage = true;
            return State.Success;
        }
    }
}