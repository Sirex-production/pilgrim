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
            return Entity.Get<EnemyStateModel>().HasDetectedNoises ? State.Success : State.Failure;
        }
    }
}