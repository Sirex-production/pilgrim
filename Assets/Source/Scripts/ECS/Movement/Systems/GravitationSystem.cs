using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Movement
{
    public sealed class GravitationSystem : IEcsRunSystem
    {
        private const float GRAVITATIONAL_FORCE_WHEN_GROUNDED = 1f;

        private readonly EcsFilter<VelocityComponent, GravityComponent, CharacterControllerModel> _gravityFilter;

        public void Run()
        {
            foreach (var i in _gravityFilter)
            {
                ref var velocityComp = ref _gravityFilter.Get1(i);
                ref var gravityComponent = ref _gravityFilter.Get2(i);
                ref var characterControllerComp = ref _gravityFilter.Get3(i);
                float maximalGravitationalForce = gravityComponent.maximalGravitationalForce;

                float gravityOffsetY = -gravityComponent.gravityAcceleration * Time.deltaTime;
                float gravityY = characterControllerComp.characterController.isGrounded ? 
                    Mathf.Clamp
                    (
                        velocityComp.velocity.y + gravityOffsetY,
                        -GRAVITATIONAL_FORCE_WHEN_GROUNDED,
                        maximalGravitationalForce
                    )
                    :
                    Mathf.Clamp
                    (
                        velocityComp.velocity.y + gravityOffsetY,
                        -maximalGravitationalForce,
                        maximalGravitationalForce
                    );

                velocityComp.velocity.y = gravityY;
            }
        }
    }
}