using System.Collections.Generic;
using Ingame.Behaviour;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Enemy
{
    public class IsCoverNearbyNode : ActionNode
    {
        [SerializeField] 
        private TypeOfCover type;

        [SerializeField] private float maxDistance = 12;
        protected override void ActOnStart()
        {
             
        }

        protected override void ActOnStop()
        {
            
        }

        protected override State ActOnTick()
        {

            /*
            ref var enemyModel = ref Entity.Get<EnemyStateModel>();
            var target = enemyModel.Target.position;
            
            float distance = float.MaxValue;
            Transform t = null;*/
//enemyModel.Covers
         
            
            
            return State.Failure;
        }

        /*private (Transform, float) GetClosestCover(HashSet<Transform> transforms, Vector3 target)
        {
            float distance = float.MaxValue;
            Transform t = null;
            foreach (var trans in transforms)
            {
                var newDistance = Vector3.Distance(target, trans.position);
                if (newDistance<distance)
                {
                    distance = newDistance;
                    t = trans;
                }
            }

            return (t, distance);
        }*/
    }
}