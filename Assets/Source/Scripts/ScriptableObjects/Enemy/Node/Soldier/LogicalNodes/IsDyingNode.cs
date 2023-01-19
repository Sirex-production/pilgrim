using Ingame.Behaviour;
using Ingame.Health;
using Leopotam.Ecs;

namespace Ingame.Enemy
{
    public sealed class IsDyingNode : ActionNode
    {
        protected override void ActOnStart()
        {
             
        }

        protected override void ActOnStop()
        {
            
        }

        protected override State ActOnTick()
        {
            ref var enemyModel = ref  entity.Get<EnemyStateModel>();
            enemyModel.isDying =  entity.Get<HealthComponent>().currentHealth<2;
            return enemyModel.isDying ? State.Success : State.Failure;
        }
    }
}