using Ingame.Behaviour;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Enemy
{
    public class StandActionNode : ActionNode
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
            ref var hitboxModel = ref Entity.Get<HitBoxCapsuleColliderModel>();
            ref var navmeshAgentModel = ref Entity.Get<NavMeshAgentModel>();

            navmeshAgentModel.Agent.speed = 5f;
            hitboxModel.capsuleCollider.center = new Vector3(0, 1.09f, 0);
            hitboxModel.capsuleCollider.radius = 0.5f;
            hitboxModel.capsuleCollider.height = 2.2f;
            
            enemyModel.isCrouching = false;
            return State.Success;
        }
    }
}