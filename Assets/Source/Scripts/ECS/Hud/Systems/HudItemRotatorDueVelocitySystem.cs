using Ingame.Data.Hud;
using Ingame.Movement;
using Ingame.Player;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Hud
{
    public sealed class HudItemRotatorDueVelocitySystem : IEcsRunSystem
    {
        private readonly EcsFilter<HudItemModel, TransformModel, InInventoryTag> _inHandItemFilter;
        private readonly EcsFilter<PlayerModel, VelocityComponent, CharacterControllerModel> _playerFilter;
        
        private const float ANGLE_FOR_ONE_SCREEN_PIXEL = .1f;
        private const float INPUT_ANGLE_VARIETY = 10f;

        public void Run()
        {
            var playerVelocity = _playerFilter.IsEmpty() ? Vector3.zero : _playerFilter.Get2(0).velocity;

            foreach (var i in _inHandItemFilter)
            {
                ref var hudItemEntity = ref _inHandItemFilter.GetEntity(i);
                ref var hudItemModel = ref _inHandItemFilter.Get1(i);
                ref var hudItemTransformModel = ref _inHandItemFilter.Get2(i);

                var itemData = hudItemModel.itemData;
                var itemTransform = hudItemTransformModel.transform;
                var itemLocalRotation = itemTransform.localRotation;
                
                var rotationDueToMovement = hudItemEntity.Has<HudIsAimingTag>() 
                    ? GetHudRotationDueToDeltaMovementWhileAiming(itemTransform.InverseTransformDirection(playerVelocity), itemData) 
                    : GetHudRotationDueToDeltaMovement(itemTransform.InverseTransformDirection(playerVelocity), itemData);
                var targetRotation = hudItemTransformModel.initialLocalRotation * rotationDueToMovement;
                var rotationSpeed = itemData.RotationSpeed * Time.deltaTime;
                
                itemLocalRotation = Quaternion.Slerp(itemLocalRotation, targetRotation, rotationSpeed);
                itemTransform.localRotation = itemLocalRotation;
            }
        }
        
        private Quaternion GetHudRotationDueToDeltaMovement(Vector3 velocity, HudItemData hudItemData)
        {
            var deltaMovementInAngle = velocity * ANGLE_FOR_ONE_SCREEN_PIXEL;
            deltaMovementInAngle.x = Mathf.Clamp(deltaMovementInAngle.x, -INPUT_ANGLE_VARIETY, INPUT_ANGLE_VARIETY);
            deltaMovementInAngle.y = Mathf.Clamp(deltaMovementInAngle.y, -INPUT_ANGLE_VARIETY, INPUT_ANGLE_VARIETY);

            if (Mathf.Abs(deltaMovementInAngle.y) < 2f * ANGLE_FOR_ONE_SCREEN_PIXEL)
                deltaMovementInAngle.y = 0;
            
            var xMovementAngle = hudItemData.RotationMovementAngleMultiplierX * deltaMovementInAngle.y;
            xMovementAngle = Mathf.Clamp(xMovementAngle, hudItemData.MinMaxRotationMovementAngleX.x, hudItemData.MinMaxRotationMovementAngleX.y);
            xMovementAngle *= hudItemData.InverseRotationMovementX;
            
            var zMovementAngle = hudItemData.RotationMovementAngleMultiplierZ * deltaMovementInAngle.x;
            zMovementAngle = Mathf.Clamp(zMovementAngle, hudItemData.MinMaxRotationMovementAngleZ.x, hudItemData.MinMaxRotationMovementAngleZ.y);
            zMovementAngle *= hudItemData.InverseRotationMovementZ;

            var resultRotation = Quaternion.AngleAxis(xMovementAngle, Vector3.right) 
                                 * Quaternion.AngleAxis(zMovementAngle, Vector3.forward);

            return resultRotation;
        }
        
        private Quaternion GetHudRotationDueToDeltaMovementWhileAiming(Vector3 velocity, HudItemData hudItemData)
        {
            var deltaMovementInAngle = velocity * ANGLE_FOR_ONE_SCREEN_PIXEL;
            deltaMovementInAngle.x = Mathf.Clamp(deltaMovementInAngle.x, -INPUT_ANGLE_VARIETY, INPUT_ANGLE_VARIETY);
            deltaMovementInAngle.y = Mathf.Clamp(deltaMovementInAngle.y, -INPUT_ANGLE_VARIETY, INPUT_ANGLE_VARIETY);

            if (Mathf.Abs(deltaMovementInAngle.y) < 2f * ANGLE_FOR_ONE_SCREEN_PIXEL)
                deltaMovementInAngle.y = 0;
            
            var xMovementAngle = hudItemData.AimRotationMovementAngleMultiplierX * deltaMovementInAngle.y;
            xMovementAngle = Mathf.Clamp(xMovementAngle, hudItemData.MinMaxAimRotationMovementAngleX.x, hudItemData.MinMaxAimRotationMovementAngleX.y);
            xMovementAngle *= hudItemData.InverseAimRotationMovementX;
            
            var zMovementAngle = hudItemData.AimRotationMovementAngleMultiplierY * deltaMovementInAngle.x;
            zMovementAngle = Mathf.Clamp(zMovementAngle, hudItemData.MINMaxAimRotationMovementAngleY.x, hudItemData.MINMaxAimRotationMovementAngleY.y);
            zMovementAngle *= hudItemData.InverseAimRotationMovementY;

            var resultRotation = Quaternion.AngleAxis(xMovementAngle, Vector3.right) 
                                 * Quaternion.AngleAxis(zMovementAngle, Vector3.up);

            return resultRotation;
        }
    }
}