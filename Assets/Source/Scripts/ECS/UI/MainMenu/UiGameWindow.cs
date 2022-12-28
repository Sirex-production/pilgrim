using NaughtyAttributes;
using Support;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Ingame.UI.MainMenu
{
	public sealed class UiGameWindow : MonoBehaviour
	{
		[BoxGroup("References")]
		[Required, SerializeField] private Button townLevelButton;
		[BoxGroup("Scene loading properties")]
		[Scene, SerializeField] private int townLevelToLoad;

		private LevelManagementService _levelManagementService;

		[Inject]
		private void Construct(LevelManagementService levelManagementService)
		{
			_levelManagementService = levelManagementService;
		}

		private void Awake()
		{
			townLevelButton.onClick.AddListener(OnTownLevelButtonClicked);
		}

		private void OnDestroy()
		{
			townLevelButton.onClick.RemoveListener(OnTownLevelButtonClicked);
		}

		private void OnTownLevelButtonClicked()
		{
			_levelManagementService.LoadLevel(townLevelToLoad);
		}
	}
}