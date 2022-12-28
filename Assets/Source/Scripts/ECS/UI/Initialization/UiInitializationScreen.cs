using NaughtyAttributes;
using Support;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Ingame.UI.Initialization
{
	public sealed class UiInitializationScreen : MonoBehaviour
	{
		[BoxGroup("References")]
		[Required, SerializeField] private Button loginButton;
		
		[BoxGroup("Properties")]
		[SerializeField] [Scene] private int loadSceneIndex;

		private LevelManagementService _levelManagementService;
		
		[Inject]
		private void Construct(LevelManagementService levelManagementService)
		{
			_levelManagementService = levelManagementService;
		}

		private void Awake()
		{
			loginButton.onClick.AddListener(OnLoginButtonClicked);
		}

		private void OnDestroy()
		{
			loginButton.onClick.RemoveListener(OnLoginButtonClicked);
		}

		private void OnLoginButtonClicked()
		{
			_levelManagementService.LoadLevel(loadSceneIndex);
		}
	}
}