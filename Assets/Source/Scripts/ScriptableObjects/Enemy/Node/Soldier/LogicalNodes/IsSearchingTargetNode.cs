using Ingame.Behaviour;
using Leopotam.Ecs;

namespace Ingame.Enemy
{
    public class IsSearchingTargetNode : ActionNode
    {
        protected override void ActOnStart()
        {
             
        }

        protected override void ActOnStop()
        {
           
        }

        protected override State ActOnTick()
        {
            return Entity.Get<EnemyStateModel>().ShouldSearchForTarget ? State.Success : State.Failure;
        }
    }
}