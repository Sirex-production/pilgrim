using Ingame.Behaviour;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Enemy
{
    public class StandActionNode : ActionNode
    {
        protected override void ActOnStart()
        {
             
        }

        protected override void ActOnStop()
        {
           
        }

        protected override State ActOnTick()
        {
            ref var enemyModel = ref entity.Get<EnemyStateModel>();
            ref var navmeshAgentModel = ref entity.Get<NavMeshAgentModel>();

            navmeshAgentModel.Agent.speed = 5f;
            enemyModel.isCrouching = false;
            
            return State.Success;
        }
    }
}