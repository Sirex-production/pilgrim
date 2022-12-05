using Ingame.Behaviour;
using Leopotam.Ecs;

namespace Ingame.Enemy
{
    public class TakeCoverActionNode : ActionNode
    {
        protected override void ActOnStart()
        {
             
        }

        protected override void ActOnStop()
        {
           
        }

        protected override State ActOnTick()
        {
            ref var navAgent = ref Entity.Get<NavMeshAgentModel>();
            ref var enemy = ref Entity.Get<EnemyStateModel>();
            
            return State.Failure;
        }
    }
}