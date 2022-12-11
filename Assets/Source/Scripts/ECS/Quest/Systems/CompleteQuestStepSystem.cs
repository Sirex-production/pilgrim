using Leopotam.Ecs;

namespace Ingame.Quests
{
	public sealed class CompleteQuestStepSystem : IEcsRunSystem
	{
		private readonly EcsWorld _world;
		private readonly EcsFilter<QuestComponent, ActiveQuestTag> _activeQuestFilter;
		private readonly EcsFilter<QuestConfigModel> _questConfigFilter;
		
		private readonly EcsFilter<CompleteQuestStepRequest> _completeQuestRequestFilter;

		public void Run()
		{
			if(_completeQuestRequestFilter.IsEmpty() || _questConfigFilter.IsEmpty())
				return;

			var questConfig = _questConfigFilter.Get1(0).questsConfig;
			ref var completeQuestStepRequest = ref _completeQuestRequestFilter.Get1(0);

			foreach (var i in _activeQuestFilter)
			{
				ref var activeQuestComponent = ref _activeQuestFilter.Get1(i);
				
				if(completeQuestStepRequest.questDefinition != activeQuestComponent.questDefinition)
					continue;

				int questTreeId = completeQuestStepRequest.questDefinition.treeId;
				int amountOfQuestSteps = questConfig.GetAmountOfStepsForTree(activeQuestComponent.questDefinition.treeId);
				
				if (questTreeId >= amountOfQuestSteps - 1)
					_world.NewEntity().Get<CompleteQuestTreeRequest>().treeId = questTreeId;
				else
					completeQuestStepRequest.questDefinition.stepId++;
				
				_world.NewEntity().Get<UpdateQuestStatesEvent>();
			}
		}
	}
}