using Ingame.Behaviour;
using Ingame.Movement;
using Leopotam.Ecs;
using UnityEngine;
using Support.Extensions;

namespace Ingame.Enemy
{
    public sealed class RagdollActionNode : WaitNode
    {
        protected override void ActOnStart()
        {
            ref var agentModel = ref Entity.Get<NavMeshAgentModel>();
            agentModel.Agent.enabled = false;
            var weapon = Entity.Get<EnemyWeaponHolderModel>().weapon;
           
            if (weapon != null && weapon.TryGetComponent<Rigidbody>(out var rb))
            {
                weapon.GetComponent<Collider>().isTrigger = false;
                weapon.transform.parent = null;
                rb.useGravity = true;
                rb.isKinematic = false;
            }
            
            Entity.Del<EnemyWeaponHolderModel>();
        }
        
        
        
        protected override void ActOnStop()
        {
            Entity.Get<HitBoxCapsuleColliderModel>().capsuleCollider.isTrigger = true;
            base.ActOnStop();
        }
        
    }
}