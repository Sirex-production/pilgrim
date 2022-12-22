using Ingame.SaveLoad;
using NaughtyAttributes;
using Support;
using UnityEngine;
using Zenject;

namespace Ingame
{
    public class ProjectContextServicesInstaller : MonoInstaller
    {
        [BoxGroup("Template components")]
        [SerializeField] private LevelManagementService levelManagementService;

        public override void InstallBindings()
        {
            var saveLoadService = new SaveLoadService();
            var stationaryInputSystem = new StationaryInput();

            stationaryInputSystem.Enable();

            Container.Bind<SaveLoadService>()
                .FromInstance(saveLoadService)
                .AsSingle()
                .NonLazy();
            
            Container.Bind<StationaryInput>()
                .FromInstance(stationaryInputSystem)
                .AsSingle()
                .NonLazy();
            
            Container.Bind<LevelManagementService>()
                .FromInstance(levelManagementService)
                .AsSingle()
                .NonLazy();
        }
    }
}