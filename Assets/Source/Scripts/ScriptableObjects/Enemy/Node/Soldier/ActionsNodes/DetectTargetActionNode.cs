using Ingame.Behaviour;
using Ingame.Movement;
using Leopotam.Ecs;
using NaughtyAttributes;
using UnityEngine;

namespace Ingame.Enemy
{
    public enum TypeOfDetection
    {
        RayCast,
        PhotoScanning
    }
    
    public class DetectTargetActionNode : ActionNode
    {
        [SerializeField] private TypeOfDetection typeOfDetection;
        [SerializeField] private float detectionRange = 45f;
        [SerializeField] private float detectionAngle = 55;
        
        private float _time;
        protected override void ActOnStart()
        { 
         
        
        }

        protected override void ActOnStop()
        {
           
        }
    
        protected override State ActOnTick()
        {
            var enemyTransform =  Entity.Get<TransformModel>().transform;
            ref var enemyModel = ref Entity.Get<EnemyStateModel>();
            var target =  enemyModel.target;
            
            if (Entity.Get<EnemyStateModel>().isTargetDetected)
            {
                return State.Success;
            }
            
            //range
            var distance = Vector3.Distance(target.position, enemyTransform.position);
            if (distance > detectionRange)
            {
                return State.Failure;
            }

            //Angle
            var dir = target.position - enemyTransform.position;
            dir.y = 0;
            var deltaAngle = Vector3.Angle(dir, enemyTransform.forward);
            if (deltaAngle >= detectionAngle || deltaAngle < 0)
            {
                return State.Failure;
            }

            var state = State.Failure;
            
            if (typeOfDetection == TypeOfDetection.RayCast)
                return GetPlayerStateFromRayCast(target, enemyTransform, distance+1, ref enemyModel);
            
            if (typeOfDetection == TypeOfDetection.PhotoScanning) 
                return GetPlayerStateFromPhotoScanning();
            
            return state;
        }

        private State GetPlayerStateFromRayCast(Transform target, Transform enemy, float distance, ref EnemyStateModel enemyStateModel) 
        {
            var direction = (target.position - enemy.position).normalized;

            var hits = Physics.RaycastAll(enemy.position, direction, distance);

            if (hits.Length == 0)
                return State.Failure;
            
            foreach (var hit in hits)
            {
                var root = hit.collider.transform.root;
                if (!root.CompareTag("Player") && root != enemy)
                    return State.Failure;
            }

            enemyStateModel.isTargetDetected = true;
            enemyStateModel.isTargetVisible = true;
       
            return State.Success;
        }
        
        private State GetPlayerStateFromPhotoScanning()
        {
            if ( Entity.Has<EnemyUseCameraRequest>())
            {
                return State.Running;
            }
            Entity.Get<EnemyUseCameraRequest>();
            return State.Running;         
        }

        private bool IsRayCastDetectionUsed() => typeOfDetection == TypeOfDetection.RayCast;
    }
}