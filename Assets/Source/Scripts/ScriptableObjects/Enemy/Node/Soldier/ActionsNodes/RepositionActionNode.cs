using System;
using Ingame.Behaviour;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.AI;

namespace Ingame.Enemy
{
    [Serializable]
    public class RepositionActionNode : ActionNode
    {
        [SerializeField] private float minDistanceBetweenEnemyAndPlayer = 6;
        private NavMeshAgent _agent;
        private Transform _target;
        protected override void ActOnStart()
        {
            _agent = entity.Get<NavMeshAgentModel>().Agent;
            _target = entity.Get<EnemyStateModel>().target;
            _agent.isStopped = false;
        }

        protected override void ActOnStop()
        {
            if (_agent == null)
            {
                entity.Get<NavMeshAgentModel>().Agent.isStopped = true;
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
            if (_agent != null && _agent.isStopped)
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
            if(_target !=null && Vector3.SqrMagnitude(_agent.transform.position-_target.position)<minDistanceBetweenEnemyAndPlayer*minDistanceBetweenEnemyAndPlayer)
            {
                _agent.velocity = Vector3.zero;
                return State.Success;
            }
            return _agent.pathStatus == NavMeshPathStatus.PathInvalid ? State.Failure : State.Running;
        }
    }
}