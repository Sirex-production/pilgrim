using Ingame.Behaviour;
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
            return Entity.Get<EnemyStateModel>().IsDying ? State.Success : State.Failure;
        }
    }
}