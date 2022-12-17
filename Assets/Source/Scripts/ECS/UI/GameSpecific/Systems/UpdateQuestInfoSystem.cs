using Ingame.Quests;
using Leopotam.Ecs;

namespace Ingame.UI
{
	public sealed class UpdateQuestInfoSystem : IEcsInitSystem, IEcsRunSystem
	{
		private EcsWorld _world;

		private readonly EcsFilter<QuestComponent, ActiveQuestTag> _activeQuestFilter;
		private readonly EcsFilter<UiGameplayQuestViewModel> _uiQuestViewFilter;
		
		private readonly EcsFilter<UpdateQuestViewsEvent> _updateQuestViewEventFilter;

		public void Init()
		{
			_world.NewEntity().Get<UpdateQuestViewsEvent>();
		}
		
		public void Run()
		{
			if(_updateQuestViewEventFilter.IsEmpty() || _activeQuestFilter.IsEmpty())
				return;

			ref var activeQuest = ref _activeQuestFilter.Get1(0);
			ref var updateQuestEventEntity = ref _updateQuestViewEventFilter.GetEntity(0);
			
			updateQuestEventEntity.Del<UpdateQuestViewsEvent>();

			foreach (var i in _uiQuestViewFilter)
			{
				var uiGameplayQuestView = _uiQuestViewFilter.Get1(i).uiGameplayQuestView;

				if(activeQuest.treeId == uiGameplayQuestView.CurrentlyDisplayedTreeId &&
				   activeQuest.stepId == uiGameplayQuestView.CurrentlyDisplayedStepId)
					continue;
				
				uiGameplayQuestView.DisplayTemporaryQuestInfo();
				uiGameplayQuestView.SetQuestInfo(activeQuest.treeId, activeQuest.stepId);
			}
		}
	}
}