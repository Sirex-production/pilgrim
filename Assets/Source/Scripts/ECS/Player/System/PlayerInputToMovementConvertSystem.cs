using Ingame.Health;
using Ingame.Input;
using Ingame.Movement;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Player
{
    public sealed class PlayerInputToMovementConvertSystem : IEcsRunSystem
    {
        private readonly EcsFilter<PlayerModel, VelocityComponent, CharacterControllerModel> _playerInputFilter;
        private readonly EcsFilter<MoveInputRequest> _moveRequestFilter;
        
        public void Run()
        {
            if(_moveRequestFilter.IsEmpty())
                return;
            
            ref var moveRequest = ref _moveRequestFilter.Get1(0);
            var inputVector = moveRequest.movementInput;
            
            foreach (var i in _playerInputFilter)
            {
                ref var playerEntity = ref _playerInputFilter.GetEntity(i);
                ref var playerModel = ref _playerInputFilter.Get1(i);
                ref var playerVelocityComponent = ref _playerInputFilter.Get2(i);
                ref var playerCharacterControllerModel = ref _playerInputFilter.Get3(i);

                var playerData = playerModel.playerMovementData;
                var playerVelocity = playerVelocityComponent.velocity;
                var playerTransform = playerCharacterControllerModel.characterController.transform;
                var movementPower = playerData.MovementAcceleration * Time.fixedDeltaTime;
                var movementDirection = playerTransform.forward * inputVector.y + 
                                        playerTransform.right * inputVector.x;

                var targetVelocity = Vector3.ClampMagnitude(movementDirection, 1) * playerModel.currentSpeed;

                if (playerEntity.Has<EnergyEffectComponent>())
                {
                    var speedBoost = playerEntity.Get<EnergyEffectComponent>().movingSpeedScale;
                    targetVelocity *= speedBoost;
                }
                
                targetVelocity.y = playerVelocity.y;

                playerVelocityComponent.velocity = Vector3.Lerp(playerVelocity, targetVelocity, movementPower);
            }
        }
    }
}