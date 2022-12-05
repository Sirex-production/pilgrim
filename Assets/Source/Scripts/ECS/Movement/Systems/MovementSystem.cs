using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Movement
{
    public sealed class MovementSystem : IEcsRunSystem
    {
        private readonly EcsFilter<VelocityComponent, CharacterControllerModel>.Exclude<BlockMovementRequest> _velocityFilter;
        
        public void Run()
        {
            foreach (var i in _velocityFilter)
            {
                ref var velocityComponent = ref _velocityFilter.Get1(i);
                ref var characterControllerModel = ref _velocityFilter.Get2(i);
                var deltaMovement = velocityComponent.velocity * Time.fixedDeltaTime;
                
                characterControllerModel.characterController.Move(deltaMovement);
            }
        }
    }
}