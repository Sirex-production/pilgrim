using Ingame.Behaviour;
using Ingame.Movement;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Enemy
{
    public sealed class SetCloserPositionToTargetActionNode : ActionNode
    {
        [SerializeField] 
        [Min(0)]
        private float stepRange;
        protected override void ActOnStart()
        {
             
        }

        protected override void ActOnStop()
        {
            
        }

        protected override State ActOnTick()
        {
            ref var enemyModel = ref entity.Get<EnemyStateModel>();
            ref var transformModel = ref entity.Get<TransformModel>();
            ref var agentModel = ref entity.Get<NavMeshAgentModel>();

            var position = transformModel.transform.position;
            var dir = (enemyModel.target.position - position).normalized;

            var newPosition = position + dir * stepRange;
            agentModel.Agent.destination = newPosition;
            return State.Success;
        }
    }
}