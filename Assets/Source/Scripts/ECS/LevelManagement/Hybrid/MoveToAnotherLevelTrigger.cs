using Ingame.Player;
using Leopotam.Ecs;
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
			if(!other.TryGetComponent(out EntityReference entityReference))
				return;
			
			if(entityReference.Entity == EcsEntity.Null)
				return;
			
			if(!entityReference.Entity.Has<PlayerModel>())
				return;
			
			_levelManagementService.LoadLevel(sceneToLoadIndex);
		}
	}
}