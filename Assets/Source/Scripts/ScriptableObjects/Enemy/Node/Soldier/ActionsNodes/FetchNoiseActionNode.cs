using Ingame.Behaviour;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Enemy
{
    public sealed class FetchNoiseActionNode : ActionNode
    {
        protected override void ActOnStart()
        {
             
        }

        protected override void ActOnStop()
        {
           
        }

        protected override State ActOnTick()
        {
            ref var enemyModel = ref Entity.Get<EnemyStateModel>();
            if (enemyModel.NoisePosition == null)
            {
                return State.Failure;
            }

            ref var agentModel = ref Entity.Get<NavMeshAgentModel>();
            agentModel.Agent.destination =  (Vector3) enemyModel.NoisePosition;
            enemyModel.HasDetectedNoises = false;
            enemyModel.NoisePosition = null;
            return State.Success;
        }
    }
}