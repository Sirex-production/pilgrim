using Leopotam.Ecs;

namespace Ingame.Quests
{
	public sealed class CompleteQuestStepSystem : IEcsRunSystem
	{
		private readonly EcsWorld _world;
		
		private readonly EcsFilter<QuestComponent> _questFilter;
		private readonly EcsFilter<QuestConfigModel> _questConfigFilter;
		private readonly EcsFilter<CompleteQuestStepRequest> _completeQuestRequestFilter;
		
		public void Run()
		{
			if(_completeQuestRequestFilter.IsEmpty() || _questConfigFilter.IsEmpty())
				return;

			var questConfig = _questConfigFilter.Get1(0).questsConfig;
			ref var completeQuestRequest = ref _completeQuestRequestFilter.Get1(0);
			ref var completeQuestEntity = ref _completeQuestRequestFilter.GetEntity(0);

			foreach (var i in _questFilter)
			{
				ref var questComp = ref _questFilter.Get1(i);

				if(completeQuestRequest.treeId != questComp.treeId)
					continue;
				
				int amountOfStepsInTree = questConfig.GetStepsCount(completeQuestRequest.treeId);

				if (questComp.stepId + 1 >= amountOfStepsInTree)
					_world.NewEntity().Get<QuestTreeIsCompltedRequest>().treeId = completeQuestRequest.treeId;
				else
					questComp.stepId++;

				_world.NewEntity().Get<UpdateQuestViewsEvent>();
			}
			
			completeQuestEntity.Del<CompleteQuestStepRequest>();
		}
	}
}