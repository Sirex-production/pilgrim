using Ingame.CameraWork;
using Ingame.Hud;
using Ingame.Interaction.Common;
using Ingame.Movement;
using Ingame.Player;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Interaction.DraggableObject
{
    public class DragObjectSystem : IEcsRunSystem
    {
        private const float DRAGGABLE_OBJECT_ADDITIONAL_DISTANCE_OFFSET = .1f;
        
        private readonly EcsFilter<RigidbodyModel, DraggableObjectModel, ObjectIsBeingDraggedTag> _draggingObjectFilter;
        private readonly EcsFilter<CameraModel, MainCameraTag> _mainCameraFilter;
        private readonly EcsFilter<PlayerModel> _playerFilter;
        
        private readonly EcsFilter<HudItemModel, InInventoryTag, HudIsInHandsTag> _visibleHudItems;

        public void Run()
        {
            if(_draggingObjectFilter.IsEmpty() || _mainCameraFilter.IsEmpty() || _playerFilter.IsEmpty())
                return;

            foreach (var i in _visibleHudItems)
            {
                ref var hudItemEntity = ref _visibleHudItems.GetEntity(i);
                
                hudItemEntity.Del<HudIsInHandsTag>();
            }
            
            ref var draggableObjectEntity = ref _draggingObjectFilter.GetEntity(0);
            ref var transformModel = ref _draggingObjectFilter.Get1(0);
            ref var draggableObjectModel = ref _draggingObjectFilter.Get2(0);
            ref var mainCamera = ref _mainCameraFilter.Get1(0);
            ref var playerData = ref _playerFilter.Get1(0).playerMovementData;

            var mainCameraTransform = mainCamera.camera.transform;
            var draggableObjectRigidbody = transformModel.rigidbody;
            var targetPos = mainCameraTransform.position + mainCameraTransform.forward * playerData.DraggableObjectDistance;

            draggableObjectRigidbody.position = Vector3.Lerp(draggableObjectRigidbody.position, targetPos, draggableObjectModel.dragSpeed * Time.deltaTime);
            draggableObjectRigidbody.angularVelocity = Vector3.zero;
            draggableObjectRigidbody.velocity = Vector3.zero;
            
            if (Vector3.Distance(targetPos, draggableObjectRigidbody.position) > playerData.DraggableObjectDistance + DRAGGABLE_OBJECT_ADDITIONAL_DISTANCE_OFFSET)
                 draggableObjectEntity.Get<PerformInteractionTag>();
        }
    }
}