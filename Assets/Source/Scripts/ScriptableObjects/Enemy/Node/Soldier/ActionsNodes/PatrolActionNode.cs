using System;
using System.Collections.Generic;
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

        [SerializeField] private float speed;
        [SerializeField] private float stoppingDistance;
        
        private int _wayPointIndex = -1;
        private Transform _point;
        private NavMeshAgent _agent;
        private Animator _animator;
        private bool _stopPatrolling = false;
        protected override void ActOnStart()
        {
            
            ref var waypoints = ref Entity.Get<WayPointsComponent>().WayPoints;
            _agent = Entity.Get<NavMeshAgentModel>().Agent;
            
            if (waypoints == null || waypoints.Count <=0)
            {
                _stopPatrolling = true;
                return;
            }
            
            AdjustWayPoints(waypoints);
            
          
            _agent.destination = _point.position;
            _agent.speed = speed;
            _agent.stoppingDistance = stoppingDistance;
            _agent.isStopped = false;
            _animator = Entity.Get<AnimatorModel>().Animator;

        }

        protected override void ActOnStop()
        {
            if (_agent == null)
            {
                return;
            }
            _agent.isStopped = true;
        }

        protected override State ActOnTick()
        {
            if (_stopPatrolling)
            {
                return State.Failure;
            }
      
            // _animator.Play("WALK_FORWARD");
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