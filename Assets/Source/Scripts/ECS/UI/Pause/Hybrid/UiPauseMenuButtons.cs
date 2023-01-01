using Ingame.Input;
using Ingame.UI.Settings;
using Leopotam.Ecs;
using NaughtyAttributes;
using Support;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Ingame.UI.Pause
{
	public sealed class UiPauseMenuButtons : MonoBehaviour
	{
		[BoxGroup("References")]
		[Required, SerializeField] private Button resumeButton;
		[BoxGroup("References")]
		[Required, SerializeField] private Button settingsButton;
		[BoxGroup("References")]
		[Required, SerializeField] private Button exitButton;
		
		[BoxGroup("Loading options")]
		[SerializeField] [Scene] private int sceneToLoadOnExit;
		
		private PauseMenuService _pauseMenuService;
		private LevelManagementService _levelManagementService;
		private UiSettingsService _uiSettingsService;
		private EcsWorld _world;
		
		[Inject]
		private void Construct(PauseMenuService pauseMenuService, LevelManagementService levelManagementService, UiSettingsService uiSettingsService, EcsWorld world)
		{
			_pauseMenuService = pauseMenuService;
			_levelManagementService = levelManagementService;
			_uiSettingsService = uiSettingsService;
			_world = world;
		}

		private void Awake()
		{
			resumeButton.onClick.AddListener(OnResumeButtonClicked);
			settingsButton.onClick.AddListener(OnSettingsButtonClicked);
			exitButton.onClick.AddListener(OnExitButtonClicked);
		}

		private void OnDestroy()
		{
			resumeButton.onClick.RemoveListener(OnResumeButtonClicked);
			settingsButton.onClick.RemoveListener(OnSettingsButtonClicked);
			exitButton.onClick.RemoveListener(OnExitButtonClicked);
		}

		private void OnResumeButtonClicked()
		{
			_world.NewEntity().Get<EnableFpsInputEvent>();	
			_pauseMenuService.RequestToHidePauseMenu();
		}
		
		private void OnSettingsButtonClicked()
		{
			_uiSettingsService.RequestOpenSettingsWindow();
		}
		
		private void OnExitButtonClicked()
		{
			_levelManagementService.LoadLevel(sceneToLoadOnExit);
		}
	}
}