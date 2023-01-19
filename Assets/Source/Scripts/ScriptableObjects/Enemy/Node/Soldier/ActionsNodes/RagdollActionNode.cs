using Ingame.Behaviour;
using Ingame.Interaction.Common;
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
            ref var agentModel = ref entity.Get<NavMeshAgentModel>();
            ref var transformModel = ref entity.Get<TransformModel>();
            
            agentModel.Agent.enabled = false;
            
            var weapon = entity.Get<EnemyWeaponHolderModel>().weapon;
            
            if(weapon == null)
               return;
            
            if ( weapon.TryGetComponent<Rigidbody>(out var rb))
            {
                weapon.GetComponent<Collider>().isTrigger = false;
                weapon.transform.parent = null;
                rb.useGravity = true;
                rb.isKinematic = false;
            }

            if (weapon.TryGetComponent<EntityReference>(out var entityReference))
            {
                entityReference.Entity.Get<InteractiveTag>();
            }
            
            entity.Del<EnemyWeaponHolderModel>();
            
            var colliders = transformModel.transform.GetComponentsInChildren<CapsuleCollider>();
            
            if(colliders == null)
                return;
            
            foreach (var collider in colliders)
            {
                collider.gameObject.AddComponent<Rigidbody>();
            }
        }

        protected override void ActOnStop()
        {
            ref var transformModel = ref entity.Get<TransformModel>();
            var colliders = transformModel.transform.GetComponentsInChildren<CapsuleCollider>();

            if (colliders == null)
                return;
            
            foreach (var collider in colliders)
            {
                collider.isTrigger = true;
                
                if(!collider.TryGetComponent<Rigidbody>(out var rb))
                    continue;
                
                Destroy(rb);
            }
        }
    }
}