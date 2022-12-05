using System;
using Ingame.Behaviour;
using Ingame.Movement;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Enemy
{
    public class IsTargetAtAngleNode : ActionNode
    {
        private enum PointToAngle
        {
            Right,
            Left,
            Front,
            Back
        }
        
        [SerializeField] 
        [Range(0,360)]
        private float angle;

        [SerializeField] 
        private PointToAngle pointDirection;
        
        protected override void ActOnStart()
        {
             
        }

        protected override void ActOnStop()
        {
        
        }

        protected override State ActOnTick()
        {
            ref var transformModel  = ref Entity.Get<TransformModel>();
            ref var enemyModel = ref Entity.Get<EnemyStateModel>();
            var target = enemyModel.Target;
            
            var dir = enemyModel.Target.position - transformModel.transform.position;
            dir.y = 0;
            dir = dir.normalized;
            
            var deltaAngle = pointDirection switch
            {
                PointToAngle.Right => Vector3.Angle(dir, -enemyModel.Target.right),
                PointToAngle.Left => Vector3.Angle(dir, enemyModel.Target.right),
                PointToAngle.Front => Vector3.Angle(dir, -enemyModel.Target.forward),
                PointToAngle.Back =>Vector3.Angle(dir, enemyModel.Target.forward),
                _ => throw new ArgumentOutOfRangeException()
            };

            return (deltaAngle >= angle || deltaAngle < 0) ? State.Failure : State.Success;
        }
    }
}