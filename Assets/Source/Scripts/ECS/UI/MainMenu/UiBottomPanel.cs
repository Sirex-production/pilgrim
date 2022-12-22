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
		[Required, SerializeField] private Button exitButton;

		private MainMenuService _mainMenuService;
		
		[Inject]
		private void Construct(MainMenuService mainMenuService)
		{
			_mainMenuService = mainMenuService;
		}

		private void Awake()
		{
			gameButton.onClick.AddListener(OnGameButtonClicked);
			developersButton.onClick.AddListener(OnDevelopersButtonClicked);
			exitButton.onClick.AddListener(OnExitButtonClicked);
		}

		private void OnDestroy()
		{
			gameButton.onClick.RemoveListener(OnGameButtonClicked);
			developersButton.onClick.RemoveListener(OnDevelopersButtonClicked);
			exitButton.onClick.RemoveListener(OnExitButtonClicked);
		}

		private void OnGameButtonClicked()
		{
			_mainMenuService.RequestWindowChange(UiWindowType.Game);
		}
		
		private void OnDevelopersButtonClicked()
		{
			_mainMenuService.RequestWindowChange(UiWindowType.Developers);
		}
		
		private void OnExitButtonClicked()
		{
			_mainMenuService.RequestWindowChange(UiWindowType.Exit);
		}
	}
}