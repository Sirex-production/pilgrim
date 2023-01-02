using Ingame.Data.Player;
using Ingame.Input;
using Ingame.Movement;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Player
{
    public sealed class PlayerInputToCrouchConverterSystem : IEcsRunSystem
    {
        private readonly EcsFilter<PlayerModel, CharacterControllerModel> _playerFilter;
        private readonly EcsFilter<CrouchInputEvent> _crouchInputFilter;
        
        public void Run()
        {
            if(_crouchInputFilter.IsEmpty())
                return;

            foreach (var i in _playerFilter)
            {
                ref var playerEntity = ref _playerFilter.GetEntity(i);
                ref var playerModel = ref _playerFilter.Get1(i);
                ref var playerCharacterControllerModel = ref _playerFilter.Get2(i);
                ref var playerCrouchRequest = ref playerEntity.Get<ChangeCharacterControllerHeightRequest>();
                var playerData = playerModel.playerMovementData;
                
                if (playerModel.isCrouching && CheckIfPlayerCanStand(playerCharacterControllerModel, playerModel.playerMovementData))
                    playerModel.isCrouching = false;
                else
                    playerModel.isCrouching = true;
                
                var targetCharacterControllerHeight = playerModel.isCrouching
                    ? playerCharacterControllerModel.initialHeight / 2
                    : playerCharacterControllerModel.initialHeight;

                playerCrouchRequest.height = targetCharacterControllerHeight;
                playerCrouchRequest.changeHeightSpeed = playerData.EnterCrouchStateSpeed;
            }
        }

        private bool CheckIfPlayerCanStand(CharacterControllerModel playerCharacterControllerModel, PlayerMovementData playerMovementData)
        {
            var characterController = playerCharacterControllerModel.characterController;
            var ray = new Ray(characterController.transform.position + characterController.center, Vector3.up);
            int layerMask = ~LayerMask.GetMask("PlayerStatic", "Ignore Raycast");
            float freeSpaceToStandUp = playerCharacterControllerModel.initialHeight * playerMovementData.AdditionalHeightMultiplierToCheckObstaclesAbove;

            return !Physics.Raycast(ray, freeSpaceToStandUp, layerMask, QueryTriggerInteraction.Ignore);
        }
    }
}