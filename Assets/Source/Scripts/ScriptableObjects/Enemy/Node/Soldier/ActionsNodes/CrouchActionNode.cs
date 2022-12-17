using Ingame.Behaviour;
using Ingame.Movement;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Enemy
{
    public class CrouchActionNode : ActionNode
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

            navmeshAgentModel.Agent.speed = 2.5f;
            hitboxModel.capsuleCollider.center = new Vector3(0, .06f, 0);
            hitboxModel.capsuleCollider.radius = 0.7f;
            hitboxModel.capsuleCollider.height = 1.7f;
            
            enemyModel.isCrouching = true;
            return State.Success;
        }
    }
}