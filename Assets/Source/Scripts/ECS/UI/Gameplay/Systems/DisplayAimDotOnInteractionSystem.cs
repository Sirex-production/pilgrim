using Ingame.CameraWork;
using Ingame.Hud;
using Ingame.Interaction.Common;
using Ingame.Player;
using Ingame.Utils;
using Leopotam.Ecs;
using Support.Extensions;
using UnityEngine;

namespace Ingame.UI
{
    public sealed class DisplayAimDotOnInteractionSystem : IEcsRunSystem
    {
        private readonly EcsWorld _world;
        private readonly EcsFilter<PlayerModel> _playerFilter;
        private readonly EcsFilter<CameraModel, MainCameraTag> _mainCameraFilter;

        private const int AMOUNT_OF_FRAMES_TO_SKIP_BETWEEN_INVOKES = 10;
        private int _framesCount = AMOUNT_OF_FRAMES_TO_SKIP_BETWEEN_INVOKES;
        
        public void Run()
        {
            if(_mainCameraFilter.IsEmpty() || _playerFilter.IsEmpty())
                return;
            
            _framesCount++;
            
            if(_framesCount < AMOUNT_OF_FRAMES_TO_SKIP_BETWEEN_INVOKES)
                return;

            float maxPlayerInteractionDistance = _playerFilter.Get1(0).playerMovementData.InteractionDistance;
            var mainCamera = _mainCameraFilter.Get1(0).camera;
            var ray = mainCamera.GetCenterScreenRay();
            bool isAimDotDisplayed = false;

            if (Physics.Raycast(ray, out RaycastHit hit, maxPlayerInteractionDistance)) // Is there any object was hit
                if (hit.collider.TryGetComponent(out EntityReference entityReference)) // Is there EntityReference on hit game object
                    if (entityReference.Entity.Has<InteractiveTag>()) // Is this entity is interactive|
                        if(!entityReference.Entity.Has<InInventoryTag>()) // Is item not in players hands
                            if(!entityReference.Entity.Has<HudIsInHandsTag>()) // Item in hands is hidden
                                isAimDotDisplayed = true;

            _world.NewEntity().Get<UpdateSettingsRequest>() = new UpdateSettingsRequest
            {
                isAimDotVisible = isAimDotDisplayed
            };
            
            _framesCount = 0;
        }
    }
}