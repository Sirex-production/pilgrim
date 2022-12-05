using Ingame.CameraWork;
using Ingame.Input;
using Ingame.Player;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Interaction.Common
{
    public sealed class LongInteractionSystem : IEcsRunSystem
    {
        private readonly EcsFilter<PlayerModel> _playerFilter;
        private readonly EcsFilter<CameraModel, MainCameraTag> _mainCameraFilter;
        private readonly EcsFilter<LongInteractionInputEvent> _interactInputFilter;

        public void Run()
        {
            if(_interactInputFilter.IsEmpty() || _playerFilter.IsEmpty() || _mainCameraFilter.IsEmpty())
                return;

            ref var playerModel = ref _playerFilter.Get1(0);
            ref var cameraModel = ref _mainCameraFilter.Get1(0);
            var camera = cameraModel.camera;

            var ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            int layerMask = ~LayerMask.GetMask("PlayerStatic", "Ignore Raycast");

            if (Physics.Raycast(ray, out RaycastHit hit, playerModel.playerMovementData.InteractionDistance, layerMask))
            {
                if (hit.collider.TryGetComponent(out EntityReference entityReference))
                {
                    var hitEntity = entityReference.Entity;

                    if (hitEntity.Has<InteractiveTag>())
                        hitEntity.Get<PerformLongInteractionTag>();
                }
            }
        }
    }
}