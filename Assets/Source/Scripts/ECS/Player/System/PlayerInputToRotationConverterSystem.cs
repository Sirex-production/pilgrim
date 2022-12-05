using Ingame.Input;
using Ingame.Movement;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Player
{
    public sealed class PlayerInputToRotationConverterSystem : IEcsRunSystem
    {
        private readonly EcsFilter<PlayerModel, CharacterControllerModel>.Exclude<BlockRotationRequest> _playerFilter;
        private readonly EcsFilter<RotateInputRequest> _rotationFilter;

        public void Run()
        {
            if(_rotationFilter.IsEmpty())
                return;

            ref var rotationRequest = ref _rotationFilter.Get1(0);
            var rotationInput = rotationRequest.rotationInput;
            
            foreach (var i in _playerFilter)
            {
                ref var playerModel = ref _playerFilter.Get1(i);
                ref var playerCharacterController = ref _playerFilter.Get2(i);
                var playerData = playerModel.playerMovementData;
                var playerTransform = playerCharacterController.characterController.transform;
                
                var xRotationAngle = rotationInput.x * playerData.Sensitivity * Time.fixedDeltaTime;
                var rotationOffset = Quaternion.AngleAxis(xRotationAngle, Vector3.up);

                playerTransform.localRotation *= rotationOffset;
            }
        }
    }
}