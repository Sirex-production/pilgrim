using Ingame.Extensions;
using Leopotam.Ecs;

namespace Ingame.Quests
{
	public sealed class ChangeActiveQuestSystem : IEcsRunSystem
	{
		private EcsWorld _world;
		
		private readonly EcsFilter<ChangeActiveQuestRequest> _changeActiveQuestReqFilter;
		private readonly EcsFilter<QuestComponent> _questsFilter;

		public void Run()
		{
			if(_changeActiveQuestReqFilter.IsEmpty())
				return;

			ref var changeActiveQuestReqEntity = ref _changeActiveQuestReqFilter.GetEntity(0);
			ref var changeActiveQuestReq = ref _changeActiveQuestReqFilter.Get1(0);

			foreach (var i in _questsFilter)
			{
				ref var questEntity = ref _questsFilter.GetEntity(i);
				ref var questComp = ref _questsFilter.Get1(i);

				if (questComp.questId == changeActiveQuestReq.questId)
					questEntity.Get<ActiveQuestTag>();
				
				if(questEntity.Has<ActiveQuestTag>())
					questEntity.Del<ActiveQuestTag>();
			}
			
			_world.SendSignal<QuestsAreUpdatedEvent>();
			
			changeActiveQuestReqEntity.Del<ChangeActiveQuestRequest>();
		}
	}
}