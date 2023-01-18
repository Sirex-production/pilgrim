using Ingame.Behaviour;
using Leopotam.Ecs;

namespace Ingame.Enemy
{
    public class IsCrouchingNode : ActionNode
    {
        protected override void ActOnStart()
        {
             
        }

        protected override void ActOnStop()
        {
           
        }

        protected override State ActOnTick()
        {
            return entity.Get<EnemyStateModel>().isCrouching ? State.Success : State.Failure;
        }
    }
}