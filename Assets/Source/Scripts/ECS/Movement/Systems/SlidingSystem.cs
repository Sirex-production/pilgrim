using UnityEngine;
using LeoEcsPhysics;
using Leopotam.Ecs;
using Support;

namespace Ingame.Movement
{
    public sealed class SlidingSystem : IEcsRunSystem
    {
        private readonly EcsFilter<VelocityComponent, CharacterControllerModel> _movableFilter;
        private readonly EcsFilter<OnControllerColliderHitEvent> _colliderHitFilter;

        public void Run()
        {
            foreach (var colliderHitI in _colliderHitFilter)
            {
                ref var controllerColliderEntity = ref _colliderHitFilter.GetEntity(colliderHitI);
                ref var controllerColliderHitEvent = ref _colliderHitFilter.Get1(colliderHitI);
                var hitNormal = controllerColliderHitEvent.hitNormal;

                foreach (var movableI in _movableFilter)
                {
                    ref var velocityComp = ref _movableFilter.Get1(movableI);
                    ref var characterControllerModel = ref _movableFilter.Get2(movableI);
                    var characterController = characterControllerModel.characterController;

                    if (characterController.gameObject == controllerColliderHitEvent.senderGameObject)
                    {
                        bool isStandingOnFlatSurface = Vector3.Angle(Vector3.up, hitNormal) <= characterController.slopeLimit && characterController.isGrounded;

                        characterControllerModel.isStandingOnFlatSurface = isStandingOnFlatSurface;

                        if (!isStandingOnFlatSurface)
                        {
                            velocityComp.velocity.x += (1f - hitNormal.y) * hitNormal.x * characterControllerModel.slidingForceModifier * Time.fixedDeltaTime;
                            velocityComp.velocity.z += (1f - hitNormal.y) * hitNormal.z * characterControllerModel.slidingForceModifier * Time.fixedDeltaTime;
                        }

                        controllerColliderEntity.Destroy();
                        
                        break;
                    }
                }
            }
        }
    }
}