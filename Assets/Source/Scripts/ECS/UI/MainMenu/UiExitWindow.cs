using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Ingame.UI.MainMenu
{
	public sealed class UiExitWindow : MonoBehaviour
	{
		[BoxGroup("References")]
		[SerializeField] private Button okButton;
		[BoxGroup("References")]
		[SerializeField] private Button noButton;

		private MainMenuService _mainMenuService;

		[Inject]
		private void Construct(MainMenuService mainMenuService)
		{
			_mainMenuService = mainMenuService;
		}

		private void Awake()
		{
			okButton.onClick.AddListener(OnOkButtonClicked);
			noButton.onClick.AddListener(OnNoButtonClicked);
		}

		private void OnDestroy()
		{
			okButton.onClick.RemoveListener(OnOkButtonClicked);
			noButton.onClick.RemoveListener(OnNoButtonClicked);
		}

		private void OnOkButtonClicked()
		{
			Application.Quit();
		}

		private void OnNoButtonClicked()
		{
			_mainMenuService.RequestWindowChange(UiWindowType.None);
		}
	}
}