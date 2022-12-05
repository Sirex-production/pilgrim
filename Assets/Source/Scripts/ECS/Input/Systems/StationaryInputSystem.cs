using System;
using System.Collections.Generic;
using Leopotam.Ecs;
using Support;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ingame.Input
{
    public sealed class StationaryInputSystem : IEcsRunSystem, IEcsInitSystem
    {
        private const float TAP_INTO_HOLD_TIME_THRESHOLD = 0.4f; 
        private EcsWorld _world;
        private StationaryInput _stationaryInputSystem;

        private bool _isDistortTheShutterPerformedThisFrame = false; 
        private bool _isLongInteractPerformedThisFrame = false;
        private bool _isDropGunInputWasPerformedThisFrame = false;
        private bool _isShowAmmountOfAmmoWasPerformedThisFrame = false;
        private float timePassedFromLastLeanInputRequest;

        private InputAction _movementInputX;
        private InputAction _movementInputY;
        private InputAction _rotationInputX;
        private InputAction _rotationInputY;
        private InputAction _jumpInput;
        private InputAction _crouchInput;
        private InputAction _leanInput;
        private InputAction _shootInput;
        private InputAction _aimInput;
        private InputAction _reloadInput;
        private InputAction _showAmountOfAmmoInput;
        private InputAction _distortTheShutterInput;
        private InputAction _shutterDelayInput;
        private InputAction _interactionInput;
        private InputAction _longInteractInput;
        private InputAction _dropGunInput;
        private InputAction _hideGunInput;
        private InputAction _openInventoryInput;
        private InputAction _firstSlotInteraction;
        private InputAction _secondSlotInteraction;
        
        private float _reloadTimer;
        private float _shutterDelayTimer;
 
        public void Init()
        {
            _movementInputX = _stationaryInputSystem.FPS.MovementX;
            _movementInputY = _stationaryInputSystem.FPS.MovementY;
            
            _rotationInputX = _stationaryInputSystem.FPS.RotationX;
            _rotationInputY = _stationaryInputSystem.FPS.RotationY;
            
            _jumpInput = _stationaryInputSystem.FPS.Jump;
            _crouchInput = _stationaryInputSystem.FPS.Crouch;
            _leanInput = _stationaryInputSystem.FPS.Lean;

            _shootInput = _stationaryInputSystem.FPS.Shoot;
            _aimInput = _stationaryInputSystem.FPS.Aim;

            _reloadInput = _stationaryInputSystem.FPS.Reload;
            _showAmountOfAmmoInput = _stationaryInputSystem.FPS.ShowAmountOfAmmo;
            _distortTheShutterInput = _stationaryInputSystem.FPS.DistortTheShutter;
            _shutterDelayInput = _stationaryInputSystem.FPS.ShutterDelay;

            _interactionInput = _stationaryInputSystem.FPS.Interact;
            _longInteractInput = _stationaryInputSystem.FPS.LongInteract;
            _dropGunInput = _stationaryInputSystem.FPS.DropGun;
            _hideGunInput = _stationaryInputSystem.FPS.HideGun;

            _openInventoryInput = _stationaryInputSystem.FPS.OpenInventory;

            _firstSlotInteraction = _stationaryInputSystem.FPS.FirstSlotInteraction;
            _secondSlotInteraction = _stationaryInputSystem.FPS.SecondSlotInteraction;

            _distortTheShutterInput.performed += OnDistortTheShutterPerformed;
            _longInteractInput.performed += OnLongInteractPerformed;
            _dropGunInput.performed += OnDropGunInputPerformed;
            _showAmountOfAmmoInput.performed += OnAmountOfAmmoInputPerformed;
            
        }

      
        private void OnDistortTheShutterPerformed(InputAction.CallbackContext callbackContext)
        {
            if(callbackContext.canceled || callbackContext.duration < .05f)
                return;

            _isDistortTheShutterPerformedThisFrame = true;
        }
        
        private void OnLongInteractPerformed(InputAction.CallbackContext callbackContext)
        {
            if(callbackContext.canceled || callbackContext.duration < .05f)
                return;

            _isLongInteractPerformedThisFrame = true;
        }

        private void OnDropGunInputPerformed(InputAction.CallbackContext callbackContext)
        {
            if(callbackContext.canceled || callbackContext.duration < .05f)
                return;

            _isDropGunInputWasPerformedThisFrame = true;
        }
        
        private void OnAmountOfAmmoInputPerformed(InputAction.CallbackContext callbackContext)
        {
            if(callbackContext.canceled || callbackContext.duration < .05f)
                return;

            _isShowAmmountOfAmmoWasPerformedThisFrame = true;
        }

        public void Run()
        {
            var movementInputVector = new Vector2(_movementInputX.ReadValue<float>(), _movementInputY.ReadValue<float>());
            var rotationInputVector = new Vector2(_rotationInputX.ReadValue<float>(), _rotationInputY.ReadValue<float>());
            bool jumpInput = _jumpInput.ReadValue<float>() > 0;
            bool crouchInput = _crouchInput.WasPressedThisFrame();
            bool shootInput = _shootInput.IsPressed();
            bool aimInput = _aimInput.WasPressedThisFrame();
            bool reloadInput = _reloadInput.WasPressedThisFrame();
            bool shutterDelayInput = _shutterDelayInput.WasPressedThisFrame();
            bool interactInput = _interactionInput.WasPressedThisFrame();
            bool hideGunInput = _hideGunInput.WasPressedThisFrame();
            bool openInventoryInput = _openInventoryInput.WasPressedThisFrame();
            bool interactWithFirstSlot = _firstSlotInteraction.WasPressedThisFrame();
            bool interactWithSecondSlot = _secondSlotInteraction.WasPressedThisFrame();
            
            var leanDirection = _leanInput.ReadValue<float>() switch
            {
                < 0 => LeanDirection.Left,
                > 0 => LeanDirection.Right,
                _ => LeanDirection.None
            };

            EcsEntity inputEntity = EcsEntity.Null;
            if (movementInputVector.sqrMagnitude > 0)
            {
                if (inputEntity == EcsEntity.Null)
                    inputEntity = _world.NewEntity();

                inputEntity.Get<MoveInputRequest>().movementInput = movementInputVector;
            }

            if (rotationInputVector.sqrMagnitude > 0)
            {
                if (inputEntity == EcsEntity.Null)
                    inputEntity = _world.NewEntity();

                inputEntity.Get<RotateInputRequest>().rotationInput = rotationInputVector;
            }

            if (jumpInput)
            {
                if (inputEntity == EcsEntity.Null)
                    inputEntity = _world.NewEntity();

                inputEntity.Get<JumpInputEvent>();
            }

            if (crouchInput)
            {
                if (inputEntity == EcsEntity.Null)
                    inputEntity = _world.NewEntity();

                inputEntity.Get<CrouchInputEvent>();
            }

            if (leanDirection != LeanDirection.None && _leanInput.WasPressedThisFrame())
            {
                if (inputEntity == EcsEntity.Null)
                    inputEntity = _world.NewEntity();

                inputEntity.Get<LeanInputRequest>().leanDirection = leanDirection;
            }

            if (shootInput)
            {
                if (inputEntity == EcsEntity.Null)
                    inputEntity = _world.NewEntity();

                inputEntity.Get<ShootInputEvent>();
            }

            if (aimInput)
            {
                if (inputEntity == EcsEntity.Null)
                    inputEntity = _world.NewEntity();

                inputEntity.Get<AimInputEvent>();
            }
            
            WasKeyTapped(_reloadInput, reloadInput, ref _reloadTimer,ref inputEntity, () =>  inputEntity.Get<MagazineSwitchInputEvent>());
            
            if (_isDistortTheShutterPerformedThisFrame)
            {
                if (inputEntity == EcsEntity.Null)
                    inputEntity = _world.NewEntity();

                inputEntity.Get<DistortTheShutterInputEvent>();
            }
            
            WasKeyTapped(_shutterDelayInput,shutterDelayInput ,ref _shutterDelayTimer ,ref inputEntity,() =>  inputEntity.Get<ShutterDelayInputEvent>());
            
            if (interactInput)
            {
                if (inputEntity == EcsEntity.Null)
                    inputEntity = _world.NewEntity();

                inputEntity.Get<InteractInputEvent>();
            }

            if (hideGunInput)
            {
                if (inputEntity == EcsEntity.Null)
                    inputEntity = _world.NewEntity();

                inputEntity.Get<HideGunInputEvent>();
            }

            if (_isLongInteractPerformedThisFrame)
            {
                if (inputEntity == EcsEntity.Null)
                    inputEntity = _world.NewEntity();

                inputEntity.Get<LongInteractionInputEvent>();
            }

            if (openInventoryInput)
            {
                if (inputEntity == EcsEntity.Null)
                    inputEntity = _world.NewEntity();

                inputEntity.Get<OpenInventoryInputEvent>();
            }

            if (interactWithFirstSlot)
            {
                if (inputEntity == EcsEntity.Null)
                    inputEntity = _world.NewEntity();

                inputEntity.Get<InteractWithFirstSlotInputEvent>();
            }
            
            if (interactWithSecondSlot)
            {
                if (inputEntity == EcsEntity.Null)
                    inputEntity = _world.NewEntity();

                inputEntity.Get<InteractWithSecondSlotInputEvent>();
            }

            if (_isDropGunInputWasPerformedThisFrame)
            {
                if (inputEntity == EcsEntity.Null)
                    inputEntity = _world.NewEntity();

                inputEntity.Get<DropWeaponInputEvent>();
            }

            if (_isShowAmmountOfAmmoWasPerformedThisFrame)
            {
                if (inputEntity == EcsEntity.Null)
                    inputEntity = _world.NewEntity();

                inputEntity.Get<ShowAmountOfAmmoInputEvent>();
            }

            _isDistortTheShutterPerformedThisFrame = false;
            _isLongInteractPerformedThisFrame = false;
            _isDropGunInputWasPerformedThisFrame = false;
            _isShowAmmountOfAmmoWasPerformedThisFrame = false;
        }
        
        private void WasKeyTapped(InputAction input,bool wasPressedThisFrame,ref float timer,ref EcsEntity entity , Action action)
        {
            if (wasPressedThisFrame)
            {
                timer = 0;
            }
            
            if (input.IsPressed())
            {
                timer += Time.deltaTime;
            }

            if (!input.WasReleasedThisFrame()) return;
            
            if (timer <TAP_INTO_HOLD_TIME_THRESHOLD )
            {
                if (entity == EcsEntity.Null)
                    entity = _world.NewEntity();
                action?.Invoke();
            }
            timer = 0;
        }
    }
}