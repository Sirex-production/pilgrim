using System;
using Leopotam.Ecs;
using UnityEngine;
using Zenject;

namespace Ingame.Quests
{
	public sealed class QuestInitializer : MonoBehaviour
	{
		[SerializeField] private int[] questIdsToInitialize;
		[SerializeField] private int idOfActiveQuest = 0;
		
		private EcsWorld _world;
		
		[Inject]
		public void Construct(EcsWorld world)
		{
			_world = world;
		}

		private void Start()
		{
			InitializeQuests();
		}

		private void InitializeQuests()
		{
			foreach (int questId in questIdsToInitialize)
			{
				var questEntity = _world.NewEntity(); 
				
				questEntity.Get<QuestComponent>().questId = questId;

				if (questId == idOfActiveQuest)
					questEntity.Get<ActiveQuestTag>();
			}

			_world.NewEntity().Get<QuestsAreUpdatedEvent>();
		}
	}
}