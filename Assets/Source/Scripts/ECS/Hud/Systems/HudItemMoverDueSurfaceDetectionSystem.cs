using Ingame.Movement;
using Ingame.Utils;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Hud
{
    public sealed class HudItemMoverDueSurfaceDetectionSystem : IEcsRunSystem
    {
        private readonly EcsFilter<SurfaceDetectorModel, HudItemModel, TransformModel, InInventoryTag> _itemFilter;

        public void Run()
        {
            foreach (var i in _itemFilter)
            {
                ref var hudItemEntity = ref _itemFilter.GetEntity(i);
                ref var hudItemModel = ref _itemFilter.Get2(i);

                ref var transformModel = ref _itemFilter.Get3(i);
                var surfaceDetector = _itemFilter.Get1(i).surfaceDetector;
                var hudItemData = hudItemModel.itemData;
                var hudItemTransform = transformModel.transform;
                var initialItemLocalPos = transformModel.initialLocalPos;
                var currentGunLocalPos = hudItemTransform.localPosition;
                
                var gunSurfaceDetectionResult = surfaceDetector.SurfaceDetectionType;
                
                if(gunSurfaceDetectionResult == SurfaceDetectionType.SameSpot)
                    return;

                var clippingSpeed = hudItemData.ClippingMovementSpeed * Time.deltaTime;
                var maxClippingOffset = hudItemEntity.Has<HudIsAimingTag>()
                    ? hudItemData.MaximumAimClippingOffset
                    : hudItemData.MaximumClippingOffset; 
                var movementDirectionZ = gunSurfaceDetectionResult == SurfaceDetectionType.Detection ? -maxClippingOffset : 0;
                var nextGunZ = initialItemLocalPos.z + movementDirectionZ;
                var currentGunZ = currentGunLocalPos.z;

                currentGunZ = Mathf.Lerp(currentGunZ, nextGunZ, clippingSpeed);
                hudItemTransform.localPosition = new Vector3(currentGunLocalPos.x, currentGunLocalPos.y, currentGunZ);
            }
        }
    }
}