using Ingame.Behaviour;
using Ingame.Movement;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Enemy
{
    public class ObserveActionNode : ActionNode
    {
        protected override void ActOnStart()
        {
             
        }

        protected override void ActOnStop()
        {
             
        }

        protected override State ActOnTick()
        {
            ref var enemyModel = ref Entity.Get<EnemyStateModel>();
            ref var transform = ref Entity.Get<TransformModel>();
            
            var lookPos = enemyModel.Target.position - transform.transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.transform.rotation = Quaternion.Slerp(transform.transform.rotation, rotation, 1.5f);
            return State.Success;
        }
    }
}