using Ingame.Behaviour;
using Leopotam.Ecs;

namespace Ingame.Enemy
{
    public sealed class UpdateTargetVisibilityActionNode : ActionNode
    {
        protected override void ActOnStart()
        {
            Entity.Get<EnemyUseCameraRequest>();
        }

        protected override void ActOnStop()
        {
             
        }

        protected override State ActOnTick()
        {
            return Entity.Has<EnemyUseCameraRequest>() ? State.Running : State.Success;
        }
    }
}