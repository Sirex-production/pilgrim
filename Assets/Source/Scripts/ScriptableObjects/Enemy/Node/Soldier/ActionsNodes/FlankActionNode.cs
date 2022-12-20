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
            float dotBetweenPlayerAndEnemy = Vector3.Dot(-enemy.target.forward,
                (transformModel.transform.position - enemy.target.position).normalized);
            
            float maxDot = dotBetweenPlayerAndEnemy;
            Transform coverPoint = null;
            
            foreach (var cover in enemy.undefinedCovers)
            {
                if (Vector3.Distance(cover.position,transformModel.transform.position)<minDistance)
                {
                    continue;
                }
                float dotBetweenPlayerAndCover = Vector3.Dot(-enemy.target.forward,
                    (cover.position - enemy.target.position).normalized);
                if (maxDot< dotBetweenPlayerAndCover)
                {
                    maxDot = dotBetweenPlayerAndCover;
                    coverPoint = cover;
                }
                
            }

            if (coverPoint == null)
            {
                float distance = Vector3.Distance(enemy.target.position, transformModel.transform.position);
                if (distance<safeDistance)
                {
                    distance = step;
                }
                var backPosition = enemy.target.position - (enemy.target.forward * distance);
                var dir = (backPosition - transformModel.transform.position).normalized;

                var dirPoint = transformModel.transform.position + dir * step;
                var point = (enemy.target.position - dirPoint).normalized*Random.Range(step,distance)+transformModel.transform.position;

                navAgent.Agent.destination = point;
                return State.Success;
            }
            
            navAgent.Agent.destination = (coverPoint.position+(enemy.target.position-coverPoint.position).normalized);
            
            return State.Success;
        }
      
    }
}