using Ingame.Data.Hud;
using Ingame.Input;
using Ingame.Movement;
using Ingame.Player;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Hud
{
    public sealed class HudItemRotatorDueDeltaRotationSystem : IEcsRunSystem
    {
        private readonly EcsFilter<HudItemModel, TransformModel, InInventoryTag> _inHandItemFilter;
        private readonly EcsFilter<RotateInputRequest> _rotateInputFilter;
        private readonly EcsFilter<PlayerModel> _playerFilter;

        private const float ANGLE_FOR_ONE_SCREEN_PIXEL = .01f;
        private const float INPUT_ANGLE_VARIETY = 10f;
        
        public void Run()
        {
            var deltaRotation = _rotateInputFilter.IsEmpty()? Vector2.zero : _rotateInputFilter.Get1(0).rotationInput;

            if (!_playerFilter.IsEmpty())
                deltaRotation *= _playerFilter.Get1(0).playerMovementData.Sensitivity;
            
            foreach (var i in _inHandItemFilter)
            {
                ref var hudItemEntity = ref _inHandItemFilter.GetEntity(i);
                ref var hudItemModel = ref _inHandItemFilter.Get1(i);
                ref var hudItemTransformModel = ref _inHandItemFilter.Get2(i);
                var itemData = hudItemModel.itemData;
                var itemTransform = hudItemTransformModel.transform;
                var itemLocalRotation = itemTransform.localRotation;
                bool isAiming = hudItemEntity.Has<HudIsAimingTag>();
                
                var targetRotation = isAiming ?
                    hudItemTransformModel.initialLocalRotation * GetHudRotationDueToDeltaRotationDuringAim(deltaRotation, itemData):
                    hudItemTransformModel.initialLocalRotation * GetHudRotationDueToDeltaRotation(deltaRotation, itemData);
               
                var rotationSpeed = isAiming ? itemData.AimRotationSpeed : itemData.RotationSpeed;
                rotationSpeed *= Time.deltaTime;
                
                itemLocalRotation = Quaternion.Slerp(itemLocalRotation, targetRotation, rotationSpeed);
                itemTransform.localRotation = itemLocalRotation;
            }
        }
        
        private Quaternion GetHudRotationDueToDeltaRotation(Vector2 deltaRotation, HudItemData hudItemData)
        {
            var deltaRotationInputInAngle = deltaRotation * ANGLE_FOR_ONE_SCREEN_PIXEL;
            deltaRotationInputInAngle.x = Mathf.Clamp(deltaRotationInputInAngle.x, -INPUT_ANGLE_VARIETY, INPUT_ANGLE_VARIETY);
            deltaRotationInputInAngle.y = Mathf.Clamp(deltaRotationInputInAngle.y, -INPUT_ANGLE_VARIETY, INPUT_ANGLE_VARIETY);
            
            var xRotationAngle = hudItemData.RotationAngleMultiplierX * deltaRotationInputInAngle.y;
            xRotationAngle = Mathf.Clamp(xRotationAngle, hudItemData.MinMaxRotationAngleX.x, hudItemData.MinMaxRotationAngleX.y);
            xRotationAngle *= hudItemData.InverseRotationX;

            var yRotationAngle = hudItemData.RotationAngleMultiplierY * deltaRotationInputInAngle.x;
            yRotationAngle = Mathf.Clamp(yRotationAngle, hudItemData.MinMaxRotationAngleY.x, hudItemData.MinMaxRotationAngleY.y);
            yRotationAngle *= hudItemData.InverseRotationY;

            var zRotationAngle = hudItemData.RotationAngleMultiplierZ * deltaRotationInputInAngle.x;
            zRotationAngle = Mathf.Clamp(zRotationAngle, hudItemData.MinMaxRotationAngleZ.x, hudItemData.MinMaxRotationAngleZ.y);
            zRotationAngle *= hudItemData.InverseRotationZ;

            var resultRotation = Quaternion.AngleAxis(xRotationAngle, Vector3.right) *
                                 Quaternion.AngleAxis(yRotationAngle, Vector3.up) *
                                 Quaternion.AngleAxis(zRotationAngle, Vector3.forward);

            return resultRotation;
        }
        
        private Quaternion GetHudRotationDueToDeltaRotationDuringAim(Vector2 deltaRotation, HudItemData hudItemData)
        {
            var deltaRotationInputInAngle = deltaRotation * ANGLE_FOR_ONE_SCREEN_PIXEL;
            deltaRotationInputInAngle.x = Mathf.Clamp(deltaRotationInputInAngle.x, -INPUT_ANGLE_VARIETY, INPUT_ANGLE_VARIETY);
            deltaRotationInputInAngle.y = Mathf.Clamp(deltaRotationInputInAngle.y, -INPUT_ANGLE_VARIETY, INPUT_ANGLE_VARIETY);
            
            var xRotationAngle = hudItemData.AimRotationAngleMultiplierX * deltaRotationInputInAngle.y;
            xRotationAngle = Mathf.Clamp(xRotationAngle, hudItemData.MinMaxAimRotationAngleX.x, hudItemData.MinMaxAimRotationAngleX.y);
            xRotationAngle *= hudItemData.InverseAimRotationX;

            var yRotationAngle = hudItemData.AimRotationAngleMultiplierY * deltaRotationInputInAngle.x;
            yRotationAngle = Mathf.Clamp(yRotationAngle, hudItemData.MinMaxAimRotationAngleY.x, hudItemData.MinMaxAimRotationAngleY.y);
            yRotationAngle *= hudItemData.InverseAimRotationY;

            var zRotationAngle = hudItemData.AimRotationAngleMultiplierZ * deltaRotationInputInAngle.x;
            zRotationAngle = Mathf.Clamp(zRotationAngle, hudItemData.MinMaxAimRotationAngleZ.x, hudItemData.MinMaxAimRotationAngleZ.y);
            zRotationAngle *= hudItemData.InverseAimRotationZ;

            var resultRotation = Quaternion.AngleAxis(xRotationAngle, Vector3.right) *
                                 Quaternion.AngleAxis(yRotationAngle, Vector3.up) *
                                 Quaternion.AngleAxis(zRotationAngle, Vector3.forward);

            return resultRotation;
        }
    }
}