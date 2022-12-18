using Ingame.Behaviour;
using Ingame.Movement;
using Leopotam.Ecs;
using Unity.Mathematics;
using UnityEngine;

namespace Ingame.Enemy
{
    public class ObserveActionNode : WaitNode
    {
        private float _offset = 70f;
        protected override State ActOnTick()
        {
            
            ref var enemyModel = ref Entity.Get<EnemyStateModel>();
            ref var transform = ref Entity.Get<TransformModel>();
            
            var targetRotation = Quaternion.LookRotation(enemyModel.target.transform.position - transform.transform.position);
            targetRotation.x = 0; 
            targetRotation.z = 0;
            targetRotation *= quaternion.Euler(0,_offset,0);
            
            transform.transform.rotation = Quaternion.Slerp(transform.transform.rotation, targetRotation, 1.5f * Time.deltaTime);
            return base.ActOnTick();
        }
    }
}