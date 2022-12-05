using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Movement
{
    public sealed class FrictionSystem : IEcsRunSystem
    {
        private readonly EcsFilter<FrictionComponent, VelocityComponent> _playerFilter;
        
        public void Run()
        {
            foreach (var i in _playerFilter)
            {
                ref var frictionComponent = ref _playerFilter.Get1(i);
                ref var velocityComponent = ref _playerFilter.Get2(i);
                
                var velocityCopy = velocityComponent.velocity;
                float playerMovementFriction = frictionComponent.frictionPower * Time.fixedDeltaTime;

                velocityComponent.velocity = Vector3.Lerp(velocityCopy, Vector3.zero, playerMovementFriction);
                velocityComponent.velocity = new Vector3
                {
                    x = velocityComponent.velocity.x,
                    y = velocityCopy.y,
                    z = velocityComponent.velocity.z
                };
            }
        }
    }
}