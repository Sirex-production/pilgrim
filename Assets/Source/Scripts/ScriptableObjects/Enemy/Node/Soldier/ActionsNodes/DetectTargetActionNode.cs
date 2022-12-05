using Ingame.Behaviour;
using Ingame.Movement;
using Leopotam.Ecs;
using NaughtyAttributes;
using UnityEngine;

namespace Ingame.Enemy
{
    public class DetectTargetActionNode : ActionNode
    {
        private enum TypeOfDetection
        {
            RayCast,
            PhotoScanning
        }

        [SerializeField] private TypeOfDetection typeOfDetection;
        [SerializeField] private float detectionRange = 45f;
        [SerializeField] private float detectionAngle = 55;
       
        [ShowIf("IsRayCastDetectionUsed")]
        [SerializeField] private LayerMask ignoredLayers;
        
        private Transform _transform;
        private Transform _target;
        private float _time;
        protected override void ActOnStart()
        { 
            _transform =  Entity.Get<TransformModel>().transform;
            ref var enemyModel = ref Entity.Get<EnemyStateModel>();
            _target =  enemyModel.Target;
        
        }

        protected override void ActOnStop()
        {
           
        }
    
        protected override State ActOnTick()
        {
            
            if (Entity.Get<EnemyStateModel>().IsTargetDetected)
            {
                return State.Success;
            }
            
            //range
            if ((_target.position - _transform.position).sqrMagnitude > detectionRange * detectionRange)
            {
                return State.Failure;
            }

            //Angle
            var dir = _target.position - _transform.position;
            dir.y = 0;
            var deltaAngle = Vector3.Angle(dir, _transform.forward);
            if (deltaAngle >= detectionAngle || deltaAngle < 0)
            {
                return State.Failure;
            }

            var state = State.Failure;
            switch (typeOfDetection)
            {
                case TypeOfDetection.RayCast:
                    state = GetPlayerStateFromRayCast();
                    break;
                
                case TypeOfDetection.PhotoScanning:
                    state = GetPlayerStateFromPhotoScanning();
                    break;
            }
      

            return state;
        }

        private State GetPlayerStateFromRayCast()
        {
            if (!Physics.Linecast(_transform.position, _target.position, out RaycastHit hit,ignoredLayers,QueryTriggerInteraction.Ignore))
                return State.Failure;
            Entity.Get<EnemyStateModel>().IsTargetDetected = true;
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