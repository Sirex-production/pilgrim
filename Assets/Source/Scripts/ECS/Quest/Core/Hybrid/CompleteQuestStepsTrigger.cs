using System;
using Ingame.ConfigProvision;
using Ingame.Extensions;
using Ingame.Player;
using Leopotam.Ecs;
using NaughtyAttributes;
using Support;
using UnityEngine;
using Zenject;

namespace Ingame.Quests
{
	[RequireComponent(typeof(Collider))]
	public sealed class CompleteQuestStepsTrigger : MonoBehaviour
	{
		[SerializeField] private QuestStepToCompleteData[] stepsToComplete;
		[SerializeField] private int[] questsToStart;
		[SerializeField] private int activeQuestId;

		private EcsWorld _world;
		private ConfigProviderService _configProviderService;
		
		[Inject]
		private void Construct(EcsWorld world, ConfigProviderService configProviderService)
		{
			_world = world;
			_configProviderService = configProviderService;
		}

		private void OnTriggerEnter(Collider other)
		{
			if(!other.TryGetComponent(out EntityReference entityReference))
				return;
			
			if(entityReference.Entity == EcsEntity.Null)
				return;
			
			if(!entityReference.Entity.Has<PlayerModel>())
				return;

			StartQuests();
			CompleteQuestSteps();
			_world.SendSignal<QuestsAreUpdatedEvent>();
			Destroy(this);
		}

		private void CompleteQuestSteps()
		{
			var questConfig = _configProviderService.QuestsConfig;

			foreach (var stepToCompleteData in stepsToComplete)
			{
				var quest = questConfig.GetQuest(stepToCompleteData.questId);
				var completedStepsBitset = new Bitset32();

				for (int i = 0; i < stepToCompleteData.stepsToComplete.Length && i < quest.steps.Length; i++)
				{
					if(!quest.steps[i].isCompletedByEnteringTrigger)
						continue;
					
					completedStepsBitset[i] = stepToCompleteData.stepsToComplete[i];
				}

				_world.SendSignal(new CompleteQuestStepsRequest
				{
					questId = stepToCompleteData.questId,
					stepsToComplete = completedStepsBitset
				});
			}
		}

		private void StartQuests()
		{
			foreach (var questId in questsToStart)
			{
				_world.SendSignal(new QuestComponent
				{
					questId = questId,
					completedSteps = new Bitset32()
				});
			}
			
			_world.SendSignal(new ChangeActiveQuestRequest
			{
				questId = activeQuestId
			});
		}
	}

	[Serializable]
	internal class QuestStepToCompleteData
	{
		public int questId;
		[AllowNesting]
		[InfoBox("Amount of steps should be less than 32 since Bitset32 is used")]
		public bool[] stepsToComplete; //Todo: Viktor - replace with bitset with property drawer
	}
}