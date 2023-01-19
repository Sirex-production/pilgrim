using System;
using NaughtyAttributes;
using Support;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Ingame.UI.Termional
{
	public sealed class UiGarageTerminal : MonoBehaviour
	{
		[BoxGroup("References")]
		[Required, SerializeField] private Button acceptMissionButton;
		[BoxGroup("Load settings")]
		[SerializeField] [Scene] private int sceneIndexToLoad;

		private LevelManagementService _levelManagementService;
		
		[Inject]
		private void Construct(LevelManagementService levelManagementService)
		{
			_levelManagementService = levelManagementService;
		}
		
		private void Awake()
		{
			acceptMissionButton.onClick.AddListener(OnAcceptMissionButtonClicked);
		}

		private void OnDestroy()
		{
			acceptMissionButton.onClick.RemoveListener(OnAcceptMissionButtonClicked);
		}

		private void OnAcceptMissionButtonClicked()
		{
			_levelManagementService.LoadLevel(sceneIndexToLoad);
		}
	}
}