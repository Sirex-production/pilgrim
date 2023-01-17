using System.Collections.Generic;
using Ingame.ConfigProvision;
using Ingame.Extensions;
using Ingame.QuestInventory;
using Leopotam.Ecs;
using Support;

namespace Ingame.Quests
{
	public sealed class CompleteQuestStepsThatRequireInventoryItemsSystem : IEcsRunSystem
	{
		private readonly EcsWorld _world;
		private readonly ConfigProviderService _configProviderService;

		private readonly EcsFilter<InventoryIsUpdatedEvent> _inventoryIsUpdatedEventFilter;
		private readonly EcsFilter<ItemModel, PickedUpItemTag> _inventoryItemsFilter;
		private readonly EcsFilter<QuestComponent>.Exclude<CompletedQuestTag> _questFilter;

		private readonly HashSet<PickableItemConfig> _inventorItemsBuffer = new();
		
		public void Run()
		{
			if(_inventoryIsUpdatedEventFilter.IsEmpty())
				return;

			ref var inventoryIsUpdatedEventEntity = ref _inventoryIsUpdatedEventFilter.GetEntity(0);
			var questConfig = _configProviderService.QuestsConfig;

			inventoryIsUpdatedEventEntity.Del<InventoryIsUpdatedEvent>();
			FillInventoryItems();
			
			if(_inventorItemsBuffer.Count < 1)
				return;

			foreach (var i in _questFilter)
			{
				ref var questComp = ref _questFilter.Get1(i);
				var questSteps = questConfig.GetQuest(questComp.questId).steps;
				var completeQuestStepsReq = new CompleteQuestStepsRequest
				{
					questId = questComp.questId,
					stepsToComplete = new Bitset32()
				};

				for (int questStepId = 0; questStepId < questSteps.Length; questStepId++)
				{
					var stepData = questSteps[questStepId];
					
					if(!stepData.isCompletedByFindingItem)
						continue;
					
					
					if (_inventorItemsBuffer.Contains(stepData.item))
						completeQuestStepsReq.stepsToComplete[questStepId] = true;
				}

				if (completeQuestStepsReq.stepsToComplete.Bitset != 0) 
					_world.SendSignal(completeQuestStepsReq);
			}
		}

		private void FillInventoryItems()
		{
			_inventorItemsBuffer.Clear();
			
			foreach (var i in _inventoryItemsFilter) 
				_inventorItemsBuffer.Add(_inventoryItemsFilter.Get1(i).itemConfig);
		}
	}
}