using Ingame.Input;
using Ingame.Movement;
using Ingame.Player;
using Ingame.Utils;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.CameraWork
{
    public class CameraBobbingSystem : IEcsRunSystem
    {
        private readonly EcsFilter<PlayerModel, DeltaMovementComponent, CharacterControllerModel> _playerModelFilter;
        private readonly EcsFilter<VirtualCameraModel, TransformModel, BobbingComponent, MainVirtualCameraTag> _mainCameraFilter;

        public void Run()
        {
            if (_playerModelFilter.IsEmpty() || _mainCameraFilter.IsEmpty())
                return;

            ref var playerModel = ref _playerModelFilter.Get1(0);
            ref var playerDeltaMovementComp = ref _playerModelFilter.Get2(0);
            ref var characterControllerModel = ref _playerModelFilter.Get3(0);
            ref var mainCameraTransformModel = ref _mainCameraFilter.Get2(0);
            ref var mainCameraBobbingComp = ref _mainCameraFilter.Get3(0);

            if(!characterControllerModel.characterController.isGrounded)
                return;
            
            var playerHudData = playerModel.playerHudData;            
            var mainCameraTransform = mainCameraTransformModel.transform;
            var mainCameraLocalPos = mainCameraTransform.localPosition;
            
            float bobbingOffset = Mathf.Sin(mainCameraBobbingComp.timeSpentTraveling);
            float deltaMovementMagnitude = playerDeltaMovementComp.deltaMovement.magnitude * playerHudData.HeadBobbingSpeedModifier;


            if (deltaMovementMagnitude > .01f)
            {
                var targetLocalPosition = new Vector3
                {
                    x = playerModel.currentLeanDirection == LeanDirection.None
                        ? Mathf.Lerp(mainCameraLocalPos.x, mainCameraTransformModel.initialLocalPos.x + bobbingOffset * playerHudData.HeadBobbingStrengthX, playerHudData.HeadBobbingLerpingSpeed * Time.fixedDeltaTime)
                        : mainCameraLocalPos.x,

                    y = mainCameraTransformModel.initialLocalPos.y + bobbingOffset * playerHudData.HeadBobbingStrengthY,

                    z = mainCameraTransformModel.initialLocalPos.z + bobbingOffset * playerHudData.HeadBobbingStrengthZ
                };

                mainCameraTransform.localPosition = targetLocalPosition;
                mainCameraBobbingComp.timeSpentTraveling += deltaMovementMagnitude;
            }
            else
            {
                mainCameraBobbingComp.timeSpentTraveling = 0;

                var targetLocalPosition = mainCameraTransformModel.initialLocalPos;
                targetLocalPosition.x = mainCameraLocalPos.x;
                
                mainCameraTransform.localPosition = Vector3.Lerp
                (
                    mainCameraTransform.localPosition,
                    playerModel.currentLeanDirection == LeanDirection.None
                        ? mainCameraTransformModel.initialLocalPos
                        : targetLocalPosition, 
                    playerHudData.HeadBobbingLerpingSpeed * Time.fixedDeltaTime
                );
            }
        }
    }
}