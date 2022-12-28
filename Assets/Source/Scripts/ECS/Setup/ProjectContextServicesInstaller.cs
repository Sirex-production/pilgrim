using Ingame.SaveLoad;
using Ingame.Settings;
using NaughtyAttributes;
using Support;
using UnityEngine;
using Zenject;

namespace Ingame
{
    public class ProjectContextServicesInstaller : MonoInstaller
    {
        [BoxGroup("Services")]
        [Required, SerializeField] private LevelManagementService levelManagementService;
        [BoxGroup("Services")]
        [Required, SerializeField] private GameSettingsService gameSettingsService;

        public override void InstallBindings()
        {
            InstallStationaryInput();
            InstallSaveLoadService();
            InstallLevelManagementService();
            InstallGameSettingsService();
        }

        private void InstallStationaryInput()
        {
            var stationaryInputSystem = new StationaryInput();
            stationaryInputSystem.Enable();

            Container.Bind<StationaryInput>()
                .FromInstance(stationaryInputSystem)
                .AsSingle()
                .NonLazy();
        }

        private void InstallSaveLoadService()
        {
            var saveLoadService = new SaveLoadService();
            saveLoadService.Initialize();
            
            Container.Bind<SaveLoadService>()
                .FromInstance(saveLoadService)
                .AsSingle()
                .NonLazy();
        }

        private void InstallLevelManagementService()
        {
            Container.Bind<LevelManagementService>()
                .FromInstance(levelManagementService)
                .AsSingle()
                .NonLazy();
        }

        private void InstallGameSettingsService()
        {
            Container.Bind<GameSettingsService>()
                .FromInstance(gameSettingsService)
                .AsSingle()
                .NonLazy();
        }
    }
}