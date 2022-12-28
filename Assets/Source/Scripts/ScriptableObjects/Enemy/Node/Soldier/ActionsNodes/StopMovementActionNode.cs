using Ingame.Behaviour;
using Leopotam.Ecs;

namespace Ingame.Enemy
{
    public class StopMovementActionNode : ActionNode
    {
        protected override void ActOnStart()
        {
   
        }

        protected override void ActOnStop()
        {
            
        }

        protected override State ActOnTick()
        {
            Entity.Get<NavMeshAgentModel>().Agent.isStopped = true;
            return State.Success;
        }
    }
}