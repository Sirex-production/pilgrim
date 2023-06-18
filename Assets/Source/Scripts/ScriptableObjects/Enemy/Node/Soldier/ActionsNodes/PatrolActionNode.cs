using System;
using System.Collections.Generic;
using Ingame.Animation;
using Ingame.Behaviour;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.AI;
using Voody.UniLeo;
using Zenject;

namespace Ingame.Enemy
{
    public sealed class PatrolActionNode : ActionNode
    {
        
        private int _wayPointIndex = -1;
        private Transform _point;
        private NavMeshAgent _agent;
        private Animator _animator;
        private bool _stopPatrolling = false;
        protected override void ActOnStart()
        {
            
            ref var waypoints = ref entity.Get<WayPointsComponent>().WayPoints;
            _agent = entity.Get<NavMeshAgentModel>().Agent;
            
            if (waypoints == null || waypoints.Count <=0)
            {
                entity.Get<EnemyStateModel>().isPatrolling = false;
                _stopPatrolling = true;
                return;
            }
            
            AdjustWayPoints(waypoints);
            
            if (_point == null)
            {
                entity.Get<EnemyStateModel>().isPatrolling = false;
                _stopPatrolling = true;
                return;
            }
            
            _agent.destination = _point.position;
            _agent.isStopped = false;
            entity.Get<EnemyStateModel>().isPatrolling = true;

        }

        protected override void ActOnStop()
        {
            if (_agent == null)
            {
                return;
            }

            entity.Get<EnemyStateModel>().isPatrolling = false;
            _agent.isStopped = true;
        }

        protected override State ActOnTick()
        {
            if (_stopPatrolling)
            {
                return State.Failure;
            }
            
            if (_agent.pathPending)
            {
                return State.Running; 
            }

            if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                return State.Success;
            }
            
            return _agent.pathStatus == NavMeshPathStatus.PathInvalid ? State.Failure : State.Running;
        }

        private void AdjustWayPoints(List<Transform> waypoints)
        {
            _wayPointIndex++;
            if (_wayPointIndex >= waypoints.Count)
            {
                _wayPointIndex = 0;
            }
            _point = waypoints[_wayPointIndex];
        }
    }
}