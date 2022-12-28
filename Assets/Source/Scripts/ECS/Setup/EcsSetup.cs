using Client;
using Ingame.Animation;
using Ingame.Anomaly;
using Ingame.Audio;
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
using Ingame.Quests;
using Ingame.SaveLoad;
using Ingame.Systems;
using Ingame.UI;
using Ingame.UI.Pause;
using Ingame.UI.Raycastable;
using Ingame.Utils;
using LeoEcsPhysics;
using Leopotam.Ecs;
using NaughtyAttributes;
using Support;
using UnityEngine;
using Voody.UniLeo;
using Zenject;

namespace Ingame
{
    public sealed class EcsSetup : MonoBehaviour
    {
        [Required, SerializeField] private QuestsConfig questsConfig;
        
        private StationaryInput _stationaryInput;
        private EcsWorld _world;
        private SaveLoadService _saveLoadService;
        private AudioController _audioController;

        private EcsSystems _updateSystems;
        private EcsSystems _fixedUpdateSystem;
        
        [Inject]
        private void Construct
        (
            StationaryInput stationaryInputSystem,
            EcsWorld world, 
            SaveLoadService saveLoadService,
            AudioController audioController,
            [Inject(Id = "UpdateSystems")] EcsSystems updateSystem, 
            [Inject(Id = "FixedUpdateSystems")] EcsSystems fixedUpdateSystem
        )
        {
            _stationaryInput = stationaryInputSystem;
            _world = world;
            _saveLoadService = saveLoadService;
            _audioController = audioController;
            _updateSystems = updateSystem;
            _fixedUpdateSystem = fixedUpdateSystem;
        }

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
                .Inject(_saveLoadService)
                .Inject(_stationaryInput)
                .Inject(_audioController)
                .Inject(questsConfig);
        }

        private void AddOneFrames()
        {
            _updateSystems
                .OneFrame<DebugRequest>()
                .OneFrame<JumpInputEvent>()
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
                .OneFrame<HideGunInputEvent>()
                .OneFrame<ShowActiveQuestInputEvent>()
                .OneFrame<ShowAllQuestsInputEvent>()
                .OneFrame<PauseInputEvent>();
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
                .Add(new SoldierAnimationSystem())
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
                //Quests
                .Add(new CompleteQuestStepSystem())
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
                .Add(new M14EbrReloadSystem())
                .Add(new Mp5ReloadSystem())
                .Add(new KrissVectorReloadSystem())
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
                .Add(new AudioSystem())
                .Add(new HealthDisplaySystem())
                .Add(new BleedingDisplaySystem())
                .Add(new GasChokeDisplaySystem())
                .Add(new EnergyEffectDisplaySystem())
                .Add(new PlayerPositionSetterSystem())
                //UI
                .Add(new InteractWithRaycastableUiSystem())
                .Add(new UpdateQuestUiSystem())
                .Add(new DisplayAmountOfAmmoInMagazineSystem())
                .Add(new DisplayQuestInfoSystem())
                .Add(new OpenHidePauseMenuSystem())
                //SupportCommunication
                //Utils
                .Add(new TimeSystem())
                .Add(new DebugSystem())
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