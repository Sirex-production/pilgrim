using Ingame.ConfigProvision;
using Leopotam.Ecs;
using UnityEngine.SceneManagement;

namespace Ingame.Settings
{
	public sealed class InitializeCursorSystem : IEcsInitSystem
	{
		private readonly ConfigProviderService _configProviderService;
		private readonly GameSettingsService _gameSettingsService;
		
		public void Init()
		{
			int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
			bool isCursorEnabled = _configProviderService.CursorConfig.IsCursorVisibleOnSceneLaunch(currentSceneIndex);

			_gameSettingsService.IsCursorEnabled = isCursorEnabled;
		}
	}
}