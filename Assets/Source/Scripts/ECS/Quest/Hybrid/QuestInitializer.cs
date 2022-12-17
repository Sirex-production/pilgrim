using System;
using Leopotam.Ecs;
using UnityEngine;
using Zenject;

namespace Ingame.Quests
{
	public sealed class QuestInitializer : MonoBehaviour
	{
		[Serializable]
		private struct QuestInitData
		{
			public int treeId;
			public int stepId;
		}

		[SerializeField] private QuestInitData[] initialQuests;
		[SerializeField] private int idOfActiveQuest;

		private EcsWorld _world;

		[Inject]
		private void Construct(EcsWorld world)
		{
			_world = world;
			
			for (int i = 0; i < initialQuests.Length; i++)
			{
				var questInitData = initialQuests[i];
				var questEntity = _world.NewEntity();
				ref var questComponent = ref questEntity.Get<QuestComponent>();

				questComponent.treeId = questInitData.treeId;
				questComponent.stepId = questInitData.stepId;
				
				if (i == idOfActiveQuest) 
					questEntity.Get<ActiveQuestTag>();
			}
		}

		private void OnValidate()
		{
			if (initialQuests == null || initialQuests.Length < 1)
			{
				idOfActiveQuest = -1;
				return;
			}

			idOfActiveQuest = Mathf.Clamp(idOfActiveQuest, 0, initialQuests.Length);
		}
	}
}