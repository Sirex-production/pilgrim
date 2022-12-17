using Ingame.Behaviour;
using Leopotam.Ecs;

namespace Ingame.Enemy
{
    public class IsTargetDetectedNode : ActionNode
    {
        protected override void ActOnStart()
        {
             
        }

        protected override void ActOnStop()
        {
          
        }

        protected override State ActOnTick()
        {
            return Entity.Get<EnemyStateModel>().isTargetDetected ? State.Success : State.Failure;
        }
    }
}