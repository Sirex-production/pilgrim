using System.Collections.Generic;
using Ingame.Quests;
using Leopotam.Ecs;

namespace Ingame.UI
{
	public sealed class UpdateQuestUiSystem : IEcsRunSystem
	{
		private readonly EcsFilter<QuestComponent> _questsFilter;
		private readonly EcsFilter<UiGameplayQuestViewModel> _questUiFilter;
		private readonly EcsFilter<QuestsAreUpdatedEvent> _questUpdateFilter;

		private readonly List<int> _questIdsBuffer = new(16);
		
		public void Run()
		{
			if(_questUpdateFilter.IsEmpty() || _questUiFilter.IsEmpty())
				return;

			var gameplayQuestUiView = _questUiFilter.Get1(0).uiGameplayQuestView;
			
			_questIdsBuffer.Clear();

			foreach (var i in _questsFilter)
			{
				ref var questEntity = ref _questsFilter.GetEntity(i);
				ref var questComp = ref _questsFilter.Get1(i);

				_questIdsBuffer.Add(questComp.questId);
				
				if (questEntity.Has<ActiveQuestTag>())
					gameplayQuestUiView.SetActiveQuestData(questComp.questId, questComp.completedSteps);
			}
			
			gameplayQuestUiView.SetAllQuestsData(_questIdsBuffer.ToArray());
		}
	}
}