using Ingame.Behaviour;
using Leopotam.Ecs;

namespace Ingame.Enemy
{
    public class IsNoiseDetectedNode : ActionNode
    {
        protected override void ActOnStart()
        {
             
        }

        protected override void ActOnStop()
        {
        }

        protected override State ActOnTick()
        {
            return entity.Get<EnemyStateModel>().hasDetectedNoises ? State.Success : State.Failure;
        }
    }
}