using Client;
using Ingame.Animation;
using Ingame.Anomaly;
using Ingame.Behaviour;
using Ingame.Breakable;
using Ingame.CameraWork;
using Ingame.Debuging;
 
using Ingame.Dialog;
using Ingame.Effects;
 
using Ingame.Gunplay;
using Ingame.Health;
using Ingame.Hud;
using Ingame.Input;
using Ingame.Interaction.Common;
using Ingame.Interaction.Doors;
using Ingame.Interaction.DraggableObject;
using Ingame.Inventory;
using Ingame.Ladder;
using Ingame.Movement;
using Ingame.Player;
using Ingame.SupportCommunication;
using Ingame.Systems;
using Ingame.UI;
using Ingame.UI.Raycastable;
using Ingame.Utils;
using LeoEcsPhysics;
using Leopotam.Ecs;
using Support;
using UnityEngine;
using Voody.UniLeo;
using Zenject;

namespace Ingame
{
    public sealed class EcsSetup : MonoBehaviour
    {
        [Inject] private GameController _gameController;
        [Inject] private StationaryInput _stationaryInput;
        [Inject] private EcsWorld _world;
        [Inject(Id = "UpdateSystems")] private EcsSystems _updateSystems;
        [Inject(Id = "FixedUpdateSystems")] private EcsSystems _fixedUpdateSystem;
#if UNITY_EDITOR
        private EcsProfiler _ecsProfiler;
#endif
        private void Awake()
        {
            Application.targetFrameRate = 240;
            
#if UNITY_EDITOR
            _ecsProfiler = new EcsProfiler(_world, new EcsWorldDebugListener(), _updateSystems, _fixedUpdateSystem);
#endif
            
            EcsPhysicsEvents.ecsWorld = _world;
            
            _updateSystems.ConvertScene();

            AddInjections();
            AddOneFrames();
            AddSystems();
            
            _updateSystems.Init();
            _fixedUpdateSystem.Init();
        }

        private void Update()
        {
            _updateSystems.Run();
        }

        private void FixedUpdate()
        {
            _fixedUpdateSystem.Run();
        }

        private void OnDestroy()
        {
#if UNITY_EDITOR
            _ecsProfiler.Dispose();
            _ecsProfiler = null;
#endif
            EcsPhysicsEvents.ecsWorld = null;
            
            _updateSystems.Destroy();
            _updateSystems = null;
            
            _fixedUpdateSystem.Destroy();
            _fixedUpdateSystem = null;
            
            _world.Destroy();
            _world = null;
        }

        private void AddInjections()
        {
            _updateSystems
                .Inject(_stationaryInput)
                .Inject(_gameController);
        }

        private void AddOneFrames()
        {
            _updateSystems
                .OneFrame<DebugRequest>()
                .OneFrame<JumpInputEvent>()
                .OneFrame<CrouchInputEvent>()
                .OneFrame<LeanInputRequest>()
                .OneFrame<MoveInputRequest>()
                .OneFrame<RotateInputRequest>()
                .OneFrame<ShootInputEvent>()
                .OneFrame<AimInputEvent>()
                .OneFrame<MagazineSwitchInputEvent>()
                .OneFrame<ShowAmountOfAmmoInputEvent>()
                .OneFrame<DistortTheShutterInputEvent>()
                .OneFrame<ShutterDelayInputEvent>()
                .OneFrame<InteractInputEvent>()
                .OneFrame<LongInteractionInputEvent>()
                .OneFrame<DropWeaponInputEvent>()
                .OneFrame<OpenInventoryInputEvent>()
                .OneFrame<InteractWithFirstSlotInputEvent>()
                .OneFrame<InteractWithSecondSlotInputEvent>()
                .OneFrame<HideGunInputEvent>();
        }

        private void AddSystems()
        {
            //Init
            _updateSystems
                .Add(new CharacterControllerInitSystem())
                .Add(new TransformModelInitSystem())
                .Add(new PlayerInitSystem())
                .Add(new AppearanceUpdateInitSystem())
                .Add(new DeltaMovementInitializeSystem());

            //Update
            _updateSystems
                .Add(new InitializeEntityReferenceSystem())
                .Add(new BehaviourBinderSystem())
                //Input
                .Add(new StationaryInputSystem())
                .Add(new PlayerInputToRotationConverterSystem())
                .Add(new PlayerHudInputToRotationConverterSystem())
                .Add(new PlayerInputToCrouchConverterSystem())
                .Add(new PlayerInputToLeanConverterSystem())
                .Add(new PlayerSpeedChangerSystem())
                //Animation 
                .Add(new HudItemSlotChooseSystem())
                .Add(new HudInputToStatesConverterSystem())
                .Add(new ShowHideHudItemSystem())
                //HUD
                .Add(new CameraInputToStatesConverterSystem())
                .Add(new MainCameraShakeEventReceiverSystem())
                .Add(new CameraShakeSystem())
                .Add(new HudBobbingSystem())
                .Add(new HudItemRotatorDueDeltaRotationSystem())
                .Add(new HudItemRotatorDueVelocitySystem())
                .Add(new HudItemMoveSystem())
                // .Add(new HudItemMoverDueSurfaceDetectionSystem())
                //AI
                .Add(new BehaviourSystem())
                .Add(new EnemyObstacleDetectionSystem())
                //Anomaly
                .Add(new AcidWaterSystem())
                //Health
                .Add(new DamageSystem())
                .Add(new StopBleedingSystem())
                .Add(new BleedingSystem())
                .Add(new StopGasChokeSystem())
                .Add(new GasChokeSystem())
                .Add(new HealingSystem())
                .Add(new ManageEnergyEffectSystem())
                .Add(new DeathSystem())
                .Add(new DestroyDeadActorsSystem())
                //Interaction
                .Add(new InteractionSystem())
                .Add(new LongInteractionSystem())
                .Add(new DoorRotationSystem())
                .Add(new PickUpDraggableObjectSystem())
                .Add(new ReleaseDraggableObjectDueToInteractionWithPlayer())
                .Add(new ReleaseDraggableObjectSystem())
                .Add(new DragObjectSystem())
                .Add(new BreakableSystem())
                .Add(new LadderSystem())
                //Gun play
                .Add(new RifleShootSystem())
                .Add(new CreateRecoilRequestSystem())
                .Add(new PerformShotSystem())
                .Add(new HudRecoilSystem())
                .Add(new HudItemAnimationSystem())
                .Add(new Ar15ReloadSystem())
                .Add(new Mp5ReloadSystem())
                .Add(new M14EbrReloadSystem())
                //Dialog
                .Add(new DialogSystem())
                .Add(new DialogCutDownDialogSystem())
                //Inventory
                .Add(new PickUpItemSystem())
                .Add(new PickUpWeaponSystem())
                .Add(new DropWeaponSystem())
                .Add(new UpdateBackpackItemsAppearanceSystem())
                .Add(new UpdateAmmoBoxViewSystem())
                .Add(new InteractWithBackpackItemSystem())
                //Effects
                .Add(new HealthDisplaySystem())
                .Add(new BleedingDisplaySystem())
                .Add(new GasChokeDisplaySystem())
                .Add(new EnergyEffectDisplaySystem())
                .Add(new PlayerPositionSetterSystem())
                //UI
                .Add(new InteractWithRaycastableUiSystem())
                .Add(new DisplayAimDotOnInteractionSystem())
                .Add(new DisplayAmountOfAmmoInMagazineSystem())
                //SupportCommunication
                .Add(new ProcessMessagesToSupportSystem())
                //Utils
                .Add(new TimeSystem())
                .Add(new DebugSystem())
                .Add(new UpdateSettingsSystem())
                .Add(new ExternalEventsRemoverSystem());

            //FixedUpdate
            _fixedUpdateSystem
                 //Input   
                .Add(new PlayerInputToMovementConvertSystem())
                 //Utils
                 .Add(new DeltaMovementCalculationSystem())
                 //Hud
                 .Add(new CameraBobbingSystem())
                 //Movement
                .Add(new FrictionSystem())
                .Add(new SlidingSystem())
                .Add(new GravitationSystem())
                .Add(new PlayerInputToJumpConverterSystem())
                .Add(new CharacterControllerHeightChangingSystem())
                .Add(new LeanSystem())
                .Add(new CameraLeanSystem())
                .Add(new MovementSystem())
                 //NoiseDetection
                 .Add(new NoiseDetectionSystem())
                 .Add(new SharedCameraDetectionSystem());
        }
    }
}