using Ingame.CameraWork;
using Ingame.Input;
using Ingame.Player;
using Leopotam.Ecs;
using Support.Extensions;
using UnityEngine;

namespace Ingame.Interaction.Common
{
    public class InteractionSystem : IEcsRunSystem
    {
        private readonly EcsFilter<PlayerModel> _playerFilter;
        private readonly EcsFilter<CameraModel, MainCameraTag> _mainCameraFilter;
        private readonly EcsFilter<InteractInputEvent> _interactInputFilter;

        public void Run()
        {
            if(_interactInputFilter.IsEmpty() || _playerFilter.IsEmpty() || _mainCameraFilter.IsEmpty())
                return;

            ref var playerModel = ref _playerFilter.Get1(0);
            ref var cameraModel = ref _mainCameraFilter.Get1(0);
            var camera = cameraModel.camera;

            var ray = camera.GetCenterScreenRay();
            int layerMask = ~LayerMask.GetMask("PlayerStatic", "Ignore Raycast");

            if (Physics.Raycast(ray, out RaycastHit hit, playerModel.playerMovementData.InteractionDistance, layerMask))
            {
                if (hit.collider.TryGetComponent(out EntityReference entityReference))
                {
                    var hitEntity = entityReference.Entity;

                    if (hitEntity.Has<InteractiveTag>())
                        hitEntity.Get<PerformInteractionTag>();
                }
            }
        }
    }
}