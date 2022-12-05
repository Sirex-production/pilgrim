using System.Collections.Generic;
using Ingame.Behaviour;
using Ingame.Behaviour.Test;
using Ingame.Movement;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ProBuilder;

namespace Ingame.Enemy
{
    public class FlankActionNode : ActionNode
    {
        [SerializeField] 
        private TypeOfCover typeOfCover;

        [SerializeField] 
        [Min(0)] 
        private float safeDistance;
        
        [SerializeField] 
        [Min(0)] 
        private float step;
        
        [SerializeField] 
        [Min(0)] 
        private float minDistance;

        [SerializeField]
        [Min(0)]
        private float maxDistance;
        protected override void ActOnStart()
        {
            
        
        }

        protected override void ActOnStop()
        {
           
        }

        protected override State ActOnTick()
        {
            ref var navAgent = ref Entity.Get<NavMeshAgentModel>();
            ref var enemy = ref Entity.Get<EnemyStateModel>();
            ref var transformModel = ref Entity.Get<TransformModel>();
            float dotBetweenPlayerAndEnemy = Vector3.Dot(-enemy.Target.forward,
                (transformModel.transform.position - enemy.Target.position).normalized);
            
            float maxDot = dotBetweenPlayerAndEnemy;
            Transform coverPoint = null;
            
            foreach (var cover in enemy.UndefinedCovers)
            {
                if (Vector3.Distance(cover.position,transformModel.transform.position)<minDistance)
                {
                    continue;
                }
                float dotBetweenPlayerAndCover = Vector3.Dot(-enemy.Target.forward,
                    (cover.position - enemy.Target.position).normalized);
                if (maxDot< dotBetweenPlayerAndCover)
                {
                    maxDot = dotBetweenPlayerAndCover;
                    coverPoint = cover;
                }
                
            }

            if (coverPoint == null)
            {
                float distance = Vector3.Distance(enemy.Target.position, transformModel.transform.position);
                if (distance<safeDistance)
                {
                    distance = step;
                }
                var backPosition = enemy.Target.position - (enemy.Target.forward * distance);
                var dir = (backPosition - transformModel.transform.position).normalized;

                var dirPoint = transformModel.transform.position + dir * step;
                var point = (enemy.Target.position - dirPoint).normalized*Random.Range(step,distance)+transformModel.transform.position;

                navAgent.Agent.destination = point;
                return State.Success;
            }
            
            navAgent.Agent.destination = (coverPoint.position+(enemy.Target.position-coverPoint.position).normalized);
            
            return State.Success;
        }
        /*
        protected override State ActOnTick()
        {     
            ref var navAgent = ref Entity.Get<NavMeshAgentModel>();
            ref var enemy = ref Entity.Get<EnemyStateModel>();
            
            Transform targetPoint = null;
            var path = new NavMeshPath();
            var backPoint = enemy.Target.position - enemy.Target.forward * safeDistance;
            //trace is not valid
            if (!NavMesh.CalculatePath(navAgent.Agent.transform.position, backPoint, navAgent.Agent.areaMask, path))
            {
                return State.Failure;
            }
            var fullDistance = CalculateRealDistance(navAgent.Agent.transform.position, path.corners);
            
            var colliders = Physics.OverlapSphere(navAgent.Agent.transform.position, maxDistance );
            var currentAngle = Vector3.Dot((enemy.Target.position-navAgent.Agent.transform.position).normalized, -enemy.Target.forward);
            
            for (int i = 0; i < colliders.Length; i++)
            {
                //is it a cover
                var coverPosition = colliders[i];
                if (!coverPosition.transform.gameObject.CompareTag("CoverPoint") || 
                    Vector3.Distance(coverPosition.transform.position,enemy.Target.position)<minDistance ||   
                    Vector3.Distance(coverPosition.transform.position,navAgent.Agent.transform.position)<minDistance )
                {
                 
                    continue;
                }
                //is path valid
                UnityEngine.Debug.Log("TOO CLOSE " +i);
                path = new NavMeshPath();
                if (!NavMesh.CalculatePath(coverPosition.transform.position, backPoint, navAgent.Agent.areaMask, path))
                {
                    continue;
                }
                //distance
                /*var distance = CalculateRealDistance(coverPosition.transform.position, path.corners);
                if (distance >= fullDistance)
                {
                    continue;
                }#1#
                //angle
                var angle = Vector3.Dot((enemy.Target.position-coverPosition.transform.position).normalized,-enemy.Target.forward);
                if (angle<= currentAngle)
                {
                    targetPoint = coverPosition.transform;
                }
                UnityEngine.Debug.Log($"Current{currentAngle}  angle {angle}");
                //fullDistance = distance;
                
            }
            
            //no proper cover
            if (colliders.Length <=0 || targetPoint == null)
            {
                //todo better destination tracking
               // var v = (backPoint);
               // navAgent.Agent.destination = v;
                return State.Failure;
            }
            //determine path
            navAgent.Agent.destination = targetPoint.position + (targetPoint.position-enemy.Target.position).normalized;
            return State.Success;
        }
        */

        /*private float CalculateRealDistance(Vector3 position, Vector3[] corners)
        {
            float dist = Vector3.Distance(position, corners[0]);
            for (var j = 1; j < corners.Length; j++)
            {
                dist += Vector3.Distance(corners[j - 1], corners[j]);
            }
            return dist;
        }*/
    }
}