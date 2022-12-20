using Ingame.Behaviour;
using Leopotam.Ecs;

namespace Ingame.Enemy
{
    public class IsTargetLostNode : ActionNode
    {
        protected override void ActOnStart()
        {
             
        }

        protected override void ActOnStop()
        {
             
        }

        protected override State ActOnTick()
        {
            return Entity.Get<EnemyStateModel>().hasLostTarget ? State.Success : State.Failure;
        }
    }
}