using System;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Movement
{
    public sealed class CharacterControllerHeightChangingSystem : IEcsRunSystem
    {
        private const float ADDITIONAL_HEIGHT_MULTIPLIER_TO_CHECK_OBSTACLES_ABOVE = 1.05f;

        private readonly EcsFilter<CharacterControllerModel, ChangeCharacterControllerHeightRequest> _crouchFilter;

        public void Run()
        {
            foreach (var i in _crouchFilter)
            {
                ref var characterControllerModel = ref _crouchFilter.Get1(i);
                ref var crouchReq = ref _crouchFilter.Get2(i);
                var characterController = characterControllerModel.characterController;

                if (crouchReq.height > characterController.height)
                {
                    if(!CheckIfThereIsAvailableSpaceToIncreaseHeight(characterControllerModel))
                        continue;
                }
                
                characterController.height = Mathf.Lerp(characterController.height, crouchReq.height, crouchReq.changeHeightSpeed * Time.deltaTime);
                if (Math.Abs(characterController.height - crouchReq.height) < .001f)
                {
                    characterController.height = crouchReq.height;
                    _crouchFilter.GetEntity(i).Del<ChangeCharacterControllerHeightRequest>();
                }
            }
        }
        
        private bool CheckIfThereIsAvailableSpaceToIncreaseHeight(CharacterControllerModel playerCharacterControllerModel)
        {
            var characterController = playerCharacterControllerModel.characterController;
            var ray = new Ray(characterController.transform.position + characterController.center, Vector3.up);
            int layerMask = ~LayerMask.GetMask("PlayerStatic", "Ignore Raycast");
            float freeSpaceToStandUp = playerCharacterControllerModel.initialHeight * ADDITIONAL_HEIGHT_MULTIPLIER_TO_CHECK_OBSTACLES_ABOVE;

            return !Physics.Raycast(ray, freeSpaceToStandUp, layerMask, QueryTriggerInteraction.Ignore);
        }
    }
}