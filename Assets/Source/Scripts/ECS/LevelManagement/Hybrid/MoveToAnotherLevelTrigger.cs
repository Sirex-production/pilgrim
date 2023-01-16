using NaughtyAttributes;
using Support;
using UnityEngine;
using Zenject;

namespace Ingame.LevelManagement
{
	[RequireComponent(typeof(Collider))]
	public sealed class MoveToAnotherLevelTrigger : MonoBehaviour
	{
		[SerializeField] [Scene] private int sceneToLoadIndex;
		
		private LevelManagementService _levelManagementService;
		
		[Inject]
		private void Construct(LevelManagementService levelManagementService)
		{
			_levelManagementService = levelManagementService;
		}

		private void OnTriggerEnter(Collider other)
		{
			_levelManagementService.LoadLevel(sceneToLoadIndex);
		}
	}
}