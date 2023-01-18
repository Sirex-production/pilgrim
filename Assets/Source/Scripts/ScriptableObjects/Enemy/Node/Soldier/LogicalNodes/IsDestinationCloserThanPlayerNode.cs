
using Ingame.Behaviour;
using UnityEngine;
using Leopotam.Ecs;

namespace Ingame.Enemy
{
    public sealed class IsDestinationCloserThanPlayerNode : ActionNode
    {
        protected override void ActOnStart()
        {
             
        }

        protected override void ActOnStop()
        {
        
        }

        protected override State ActOnTick()
        {
            ref var navMeshAgentModel = ref entity.Get<NavMeshAgentModel>();
            ref var enemyStateModel = ref entity.Get<EnemyStateModel>();

            if (navMeshAgentModel.Agent.path == null)
                return State.Failure;
            
            var path = navMeshAgentModel.Agent.path;
            float pathLength = 0f;
            
            for (int i = 1; i < path.corners.Length; i++)
            {
                pathLength += Vector3.Distance( path.corners[i-1], path.corners[i] );
            }

            float distanceBetweenPlayerAndEnemy = Vector3.Distance(enemyStateModel.target.position,
                navMeshAgentModel.Agent.transform.position);

            return distanceBetweenPlayerAndEnemy > pathLength ? State.Success : State.Failure;
        }
    }
}