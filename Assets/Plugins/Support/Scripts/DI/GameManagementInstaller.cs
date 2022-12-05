using NaughtyAttributes;
using Support.SLS;
using Support.UI;
using UnityEngine;
using Zenject;

namespace Support.DI.Installers
{
    public class GameManagementInstaller : MonoInstaller
    {
        [BoxGroup("Template components")]
        [SerializeField] private GameController gameController;
        [BoxGroup("Template components")]
        [SerializeField] private SaveLoadSystem saveLoadSystem;
        [BoxGroup("Template components")]
        [SerializeField] private LevelManager levelManager;
        [BoxGroup("Template components")]
        [SerializeField] private VFXController vfxController;
        [BoxGroup("Template components")]
        [SerializeField] private UiController uiController;
        
        public override void InstallBindings()
        {
            var stationaryInputSystem = new StationaryInput();
            stationaryInputSystem.Enable();
            
            Container.Bind<StationaryInput>()
                .FromInstance(stationaryInputSystem)
                .AsSingle()
                .NonLazy();

            Container.Bind<GameController>()
                .FromInstance(gameController)
                .AsSingle()
                .NonLazy();
            
            Container.Bind<SaveLoadSystem>()
                .FromInstance(saveLoadSystem)
                .AsSingle()
                .NonLazy();
            
            Container.Bind<LevelManager>()
                .FromInstance(levelManager)
                .AsSingle()
                .NonLazy();
            
            Container.Bind<VFXController>()
                .FromInstance(vfxController)
                .AsSingle()
                .NonLazy();
            
            Container.Bind<UiController>()
                .FromInstance(uiController)
                .AsSingle()
                .NonLazy();
        }
    }
}