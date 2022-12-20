using System;
using Ingame.Behaviour;
using Ingame.Movement;
using Leopotam.Ecs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

namespace Ingame.Enemy
{
    [Serializable]
    public class RepositionActionNode : ActionNode
    {
        private enum TypeOfReposition
        {
            LookAtPoint,
            LookAtTarget
        }

        [SerializeField] 
        private TypeOfReposition _typeOfReposition;
        
        private NavMeshAgent _agent;
        protected override void ActOnStart()
        {
            _agent = Entity.Get<NavMeshAgentModel>().Agent;
            _agent.isStopped = false;
        }

        protected override void ActOnStop()
        {
            if (_agent == null)
            {
                Entity.Get<NavMeshAgentModel>().Agent.isStopped = true;
            }
            else
            {
                _agent.isStopped = true;
            }
        }

        protected override State ActOnTick()
        {
            #if UNITY_EDITOR
                UnityEngine.Debug.DrawLine(_agent.transform.position,_agent.destination,Color.green);
            #endif
            if (_agent != null && _agent.isStopped )
            {
                _agent.isStopped = false;
            }
            if (_typeOfReposition == TypeOfReposition.LookAtTarget)
            {
                ref var enemyModel = ref Entity.Get<EnemyStateModel>();
                ref var transform = ref Entity.Get<TransformModel>();
            
                var targetRotation = Quaternion.LookRotation(enemyModel.target.transform.position - transform.transform.position);
                targetRotation.x = 0; 
                targetRotation.z = 0;
                transform.transform.rotation = Quaternion.Slerp(transform.transform.rotation, targetRotation, 1.5f);
                
            }
            
            if (_agent.pathPending)
            {
                return State.Running; 
            }
            
            if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                _agent.velocity = Vector3.zero;
                return State.Success;
            }
            
            return _agent.pathStatus == NavMeshPathStatus.PathInvalid ? State.Failure : State.Running;
        }
    }
}