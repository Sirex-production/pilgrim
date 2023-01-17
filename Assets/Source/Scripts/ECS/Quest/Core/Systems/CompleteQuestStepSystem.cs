using System.Runtime.CompilerServices;
using Leopotam.Ecs;
using Support;

namespace Ingame.Quests
{
	public sealed class CompleteQuestStepSystem : IEcsRunSystem
	{
		private readonly EcsWorld _world;
		private readonly QuestsConfig _questsConfig;
		
		private readonly EcsFilter<QuestComponent> _questFilter;
		private readonly EcsFilter<CompleteQuestStepsRequest> _completeQuestStepRequestFilter;

		public void Run()
		{
			if(_completeQuestStepRequestFilter.IsEmpty())
				return;
			
			ref var requestEntity = ref _completeQuestStepRequestFilter.GetEntity(0);
			ref var completeQuestStepRequest = ref _completeQuestStepRequestFilter.Get1(0);
			
			foreach (var i in _questFilter)
			{
				ref var questEntity = ref _questFilter.GetEntity(i);
				ref var questComp = ref _questFilter.Get1(i);

				if(questComp.questId != completeQuestStepRequest.questId)
					continue;

				questComp.completedSteps |= completeQuestStepRequest.stepsToComplete;

				if (AreAllStepsCompleted(questComp.completedSteps, _questsConfig.GetStepsCount(questComp.questId)))
					questEntity.Get<CompletedQuestTag>();

				_world.NewEntity().Get<QuestsAreUpdatedEvent>();
			}
			
			requestEntity.Del<CompleteQuestStepsRequest>();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool AreAllStepsCompleted(in Bitset32 completedStepsBitset, in int amountOfSteps)
		{
			for (int i = 0; i < amountOfSteps; i++)
			{
				if (completedStepsBitset[i] == false)
					return false;
			}

			return true;
		}
	}
}