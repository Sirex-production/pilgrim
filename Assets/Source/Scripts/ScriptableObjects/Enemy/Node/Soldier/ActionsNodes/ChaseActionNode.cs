using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Enemy
{
    public class ChaseActionNode : RepositionActionNode
    {
        [SerializeField] 
        [Min(0)]
        private int visibility;
        [SerializeField]
        [Min(0)]
        private float distance;
        
        protected override State ActOnTick()
        {
            ref var enemyModel = ref Entity.Get<EnemyStateModel>();
            ref var agentModel = ref  Entity.Get<NavMeshAgentModel>();
            agentModel.Agent.destination = enemyModel.target.position;

            if (enemyModel.visibleTargetPixels >= visibility)
            {
                return State.Success;
            }

            if (Vector3.Distance(enemyModel.target.position, agentModel.Agent.transform.position)<= distance)
            {
                return State.Success;
            }
            
            return base.ActOnTick();
        }
    }
}