using Ingame.Behaviour;
using Ingame.Movement;
using Leopotam.Ecs;
using Unity.Mathematics;

namespace Ingame.Enemy
{
    public class ObserveTargetActionNode : WaitNode
    {
        private float _offset = 70f;
        protected override State ActOnTick()
        {
            ref var enemyModel = ref Entity.Get<EnemyStateModel>();
            ref var transformModel = ref Entity.Get<TransformModel>();

            transformModel.transform.LookAt(enemyModel.target);
            transformModel.transform.rotation *= quaternion.Euler(0,_offset,0);
            
            return base.ActOnTick();
        }
    }
}