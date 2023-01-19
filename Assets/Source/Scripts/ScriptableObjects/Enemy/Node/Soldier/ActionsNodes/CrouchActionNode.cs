using Ingame.Behaviour;
using Ingame.Movement;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Enemy
{
    public class CrouchActionNode : ActionNode
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

            navmeshAgentModel.Agent.speed = 2.5f;
            enemyModel.isCrouching = true;
            
            return State.Success;
        }
    }
}