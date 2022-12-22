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