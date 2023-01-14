using Ingame.Behaviour;
using Ingame.Movement;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Enemy
{
    public sealed class UpdateTargetVisibilityActionNode : ActionNode
    {
        [SerializeField] 
        private TypeOfDetection typeOfDetection = TypeOfDetection.PhotoScanning;
        protected override void ActOnStart()
        {
            if(typeOfDetection == TypeOfDetection.PhotoScanning)
                Entity.Get<EnemyUseCameraRequest>();
        }

        protected override void ActOnStop()
        {
             
        }

        protected override State ActOnTick()
        {
            if(typeOfDetection == TypeOfDetection.PhotoScanning)
                return Entity.Has<EnemyUseCameraRequest>() ? State.Running : State.Success;

            if (typeOfDetection == TypeOfDetection.RayCast)
                return ActOnRayeCasting();
    
            return State.Success;
        }
        
        private State ActOnRayeCasting( )
        {
            ref var enemyStateModel = ref Entity.Get<EnemyStateModel>();
            var enemy = Entity.Get<TransformModel>().transform;
            var target = enemyStateModel.target;
            var distance = Vector3.Distance(enemy.position, target.position);
            
            var direction = (target.position - enemy.position).normalized;

            var hits = Physics.RaycastAll(enemy.position, direction, distance);

            if (hits.Length == 0)
            {
                enemyStateModel.isTargetVisible = false;
                return State.Failure;
            }

            foreach (var hit in hits)
            {
                var root = hit.collider.transform.root;
                if (!root.CompareTag("Player") && root != enemy)
                {
                    enemyStateModel.isTargetVisible = false;
                    return State.Failure;
                }
            }
            
            enemyStateModel.isTargetVisible = true;
       
            return State.Success;
        }
    }
}