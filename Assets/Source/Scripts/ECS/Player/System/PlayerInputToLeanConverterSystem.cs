using Ingame.CameraWork;
using Ingame.Hud;
using Ingame.Input;
using Ingame.Movement;
using Ingame.Player;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Player
{
    public sealed class PlayerInputToLeanConverterSystem : IEcsRunSystem
    {
        private EcsFilter<PlayerModel> _playerFilter;
        private EcsFilter<HudLeanOriginTag> _leanOriginFilter;
        private EcsFilter<MainVirtualCameraTag> _mainCameraFilter;
        private EcsFilter<LeanInputRequest> _leanInputRequestFilter;

        public void Run()
        {
            if(_leanInputRequestFilter.IsEmpty() || _playerFilter.IsEmpty())
                return;

            ref var playerModel = ref _playerFilter.Get1(0);
            var playerData = playerModel.playerMovementData;
            var leanDirectionInput = _leanInputRequestFilter.Get1(0).leanDirection;
            
            playerModel.currentLeanDirection = playerModel.currentLeanDirection == leanDirectionInput ?
                LeanDirection.None : 
                leanDirectionInput;

            foreach (var i in _leanOriginFilter)
            {
                ref var leanOriginEntity = ref _leanOriginFilter.GetEntity(i);
                ref var leanCallback = ref leanOriginEntity.Get<LeanCallback>();

                leanCallback.rotationAxis = Vector3.forward;
                leanCallback.angle = playerModel.currentLeanDirection switch
                {
                    LeanDirection.Left => playerData.LeanAngleOffset,
                    LeanDirection.Right => -playerData.LeanAngleOffset,
                    _ => 0
                };

                leanCallback.speed = playerData.EnterLeanSpeed;
            }

            foreach (var i in _mainCameraFilter)
            {
                ref var mainCameraEntity = ref _mainCameraFilter.GetEntity(i);
                ref var cameraLeanCallback = ref mainCameraEntity.Get<CameraLeanCallback>();
                
                cameraLeanCallback.positionOffset = playerModel.currentLeanDirection switch
                {
                    LeanDirection.Left => Vector3.left * playerData.CameraPositionOffsetDuringTheLeftLean,
                    LeanDirection.Right => Vector3.right * playerData.CameraPositionOffsetDuringTheRightLean,
                    _ => Vector3.zero
                };

                cameraLeanCallback.enterLeanSpeed = playerData.EnterLeanSpeed;
            }
        }
    }
}