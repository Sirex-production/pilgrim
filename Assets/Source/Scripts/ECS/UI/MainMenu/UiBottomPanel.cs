using Ingame.UI.Settings;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Ingame.UI.MainMenu
{
	public sealed class UiBottomPanel : MonoBehaviour
	{
		[BoxGroup("References")]
		[Required, SerializeField] private Button gameButton;
		[BoxGroup("References")]
		[Required, SerializeField] private Button developersButton;
		[BoxGroup("References")]
		[Required, SerializeField] private Button settingsButton;
		[BoxGroup("References")]
		[Required, SerializeField] private Button exitButton;

		private MainMenuService _mainMenuService;
		private UiSettingsService _uiSettingsService;
		
		[Inject]
		private void Construct(MainMenuService mainMenuService, UiSettingsService uiSettingsService)
		{
			_mainMenuService = mainMenuService;
			_uiSettingsService = uiSettingsService;
		}

		private void Awake()
		{
			gameButton.onClick.AddListener(OnGameButtonClicked);
			developersButton.onClick.AddListener(OnDevelopersButtonClicked);
			settingsButton.onClick.AddListener(OnSettingsButtonClicked);
			exitButton.onClick.AddListener(OnExitButtonClicked);
		}

		private void OnDestroy()
		{
			gameButton.onClick.RemoveListener(OnGameButtonClicked);
			developersButton.onClick.RemoveListener(OnDevelopersButtonClicked);
			settingsButton.onClick.RemoveListener(OnSettingsButtonClicked);
			exitButton.onClick.RemoveListener(OnExitButtonClicked);
		}

		private void OnGameButtonClicked()
		{
			_mainMenuService.RequestWindowChange(UiWindowType.Game);
			_uiSettingsService.RequestCloseSettingsWindow();
		}
		
		private void OnDevelopersButtonClicked()
		{
			_mainMenuService.RequestWindowChange(UiWindowType.Developers);
			_uiSettingsService.RequestCloseSettingsWindow();
		}

		private void OnSettingsButtonClicked()
		{
			_uiSettingsService.RequestOpenSettingsWindow();
		}

		private void OnExitButtonClicked()
		{
			_mainMenuService.RequestWindowChange(UiWindowType.Exit);
			_uiSettingsService.RequestCloseSettingsWindow();
		}
	}
}