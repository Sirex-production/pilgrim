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
            ref var enemyModel = ref  Entity.Get<EnemyStateModel>();
            enemyModel.isDying =  Entity.Has<DeathTag>();
            return enemyModel.isDying ? State.Success : State.Failure;
        }
    }
}