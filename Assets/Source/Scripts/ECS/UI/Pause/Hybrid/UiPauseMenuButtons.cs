using DG.Tweening;
using Ingame.Input;
using Ingame.UI.Settings;
using Leopotam.Ecs;
using NaughtyAttributes;
using Support;
using Support.Extensions;
using UnityEngine;
using UnityEngine.Serialization;
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
		[Required, SerializeField] private Button controlsButton;
		[BoxGroup("References")]
		[Required, SerializeField] private Button controlsBackButton;
		[BoxGroup("References")]
		[Required, SerializeField] private Button exitButton;
		[FormerlySerializedAs("controlsScreen")]
		[BoxGroup("References")]
		[Required, SerializeField] private CanvasGroup controlsScreenCanvasGroup;
		
		[BoxGroup("Loading options")]
		[SerializeField] [Scene] private int sceneToLoadOnExit;
		
		[BoxGroup("Animation options")]
		[SerializeField] [Min(0)] private float showAnimationDuration = .3f;
		
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
			controlsButton.onClick.AddListener(OnControlsButtonClicked);
			controlsBackButton.onClick.AddListener(OnControlsBackButtonClicked);
			exitButton.onClick.AddListener(OnExitButtonClicked);
			
			OnControlsBackButtonClicked();
		}

		private void OnDestroy()
		{
			resumeButton.onClick.RemoveListener(OnResumeButtonClicked);
			settingsButton.onClick.RemoveListener(OnSettingsButtonClicked);
			controlsButton.onClick.RemoveListener(OnControlsButtonClicked);
			controlsBackButton.onClick.RemoveListener(OnControlsBackButtonClicked);
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

		private void OnControlsButtonClicked()
		{
			controlsScreenCanvasGroup.SetGameObjectActive();
			controlsScreenCanvasGroup.DOFade(1, showAnimationDuration);
		}

		private void OnControlsBackButtonClicked()
		{
			controlsScreenCanvasGroup.DOFade(0, showAnimationDuration)
				.OnComplete(controlsScreenCanvasGroup.SetGameObjectInactive);
		}

		private void OnExitButtonClicked()
		{
			_levelManagementService.LoadLevel(sceneToLoadOnExit);
		}
	}
}