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
		[Required, SerializeField] private Button newGameButton;
		[BoxGroup("References")]
		[Required, SerializeField] private Button garageLevelButton;
		[BoxGroup("References")]
		[Required, SerializeField] private Button townLevelButton;
		[BoxGroup("References")]
		[Required, SerializeField] private Button forestLevelButton;
		
		[BoxGroup("Scene loading properties")]
		[Scene, SerializeField] private int newGameLevelToLoad;
		[BoxGroup("Scene loading properties")]
		[Scene, SerializeField] private int garageLevelToLoad;
		[BoxGroup("Scene loading properties")]
		[Scene, SerializeField] private int townLevelToLoad;
		[BoxGroup("Scene loading properties")]
		[Scene, SerializeField] private int forestLevelToLoad;

		private LevelManagementService _levelManagementService;

		[Inject]
		private void Construct(LevelManagementService levelManagementService)
		{
			_levelManagementService = levelManagementService;
		}

		private void Awake()
		{
			newGameButton.onClick.AddListener(OnNewGameButtonClicked);
			garageLevelButton.onClick.AddListener(OnGarageLevelButtonClicked);
			townLevelButton.onClick.AddListener(OnTownLevelButtonClicked);
			forestLevelButton.onClick.AddListener(OnForestLevelButtonClicked);
		}

		private void OnDestroy()
		{
			newGameButton.onClick.RemoveListener(OnNewGameButtonClicked);
			garageLevelButton.onClick.RemoveListener(OnGarageLevelButtonClicked);
			townLevelButton.onClick.RemoveListener(OnTownLevelButtonClicked);
			forestLevelButton.onClick.RemoveListener(OnForestLevelButtonClicked);
		}

		private void OnNewGameButtonClicked()
		{
			_levelManagementService.LoadLevel(newGameLevelToLoad);
		}
		
		private void OnGarageLevelButtonClicked()
		{
			_levelManagementService.LoadLevel(garageLevelToLoad);
		}
		
		private void OnTownLevelButtonClicked()
		{
			_levelManagementService.LoadLevel(townLevelToLoad);
		}
		
		private void OnForestLevelButtonClicked()
		{
			_levelManagementService.LoadLevel(forestLevelToLoad);
		}
	}
}