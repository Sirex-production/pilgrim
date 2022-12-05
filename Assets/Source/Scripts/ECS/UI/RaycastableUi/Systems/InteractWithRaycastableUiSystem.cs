using Ingame.CameraWork;
using Ingame.Input;
using Ingame.Player;
using Leopotam.Ecs;
using UnityEngine;
using Screen = UnityEngine.Device.Screen;

namespace Ingame.UI.Raycastable
{
    public sealed class InteractWithRaycastableUiSystem : IEcsRunSystem
    {
        private readonly EcsFilter<CameraModel, MainCameraTag> _mainCameraFilter;
        private readonly EcsFilter<PlayerModel> _playerModel;
        private readonly EcsFilter<InteractInputEvent> _interactInputEventFilter;

        public void Run()
        {
            if(_playerModel.IsEmpty())
                return;

            var playerMovementData = _playerModel.Get1(0).playerMovementData;
            
            foreach (var i in _mainCameraFilter)
            {
                ref var mainCamera = ref _mainCameraFilter.Get1(i);
                var screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
                var centerCameraRay = mainCamera.camera.ScreenPointToRay(screenCenter);

                if (Physics.Raycast(centerCameraRay, out RaycastHit hit, playerMovementData.InteractionDistance))
                {
                    if(!hit.collider.TryGetComponent(out RaycastableButton raycastableButton))
                        continue;
                    
                    raycastableButton.Select();
                    
                    if (!_interactInputEventFilter.IsEmpty()) 
                        raycastableButton.Press();
                }
            }
        }
    }
}