using Ingame.Quests;
using Leopotam.Ecs;

namespace Ingame.UI
{
	public sealed class DisplayQuestCompletionSystem : IEcsRunSystem
	{
		private readonly EcsFilter<CompleteQuestStepRequest> _completeQuestRequestFilter;
		private readonly EcsFilter<UiGameplayQuestViewModel> _uiGameplayQuestViewModelFilter;

		public void Run()
		{
			if(_completeQuestRequestFilter.IsEmpty())
				return;

			ref var completeQuestStepRequest = ref _completeQuestRequestFilter.Get1(0);
			
			foreach (var i in _uiGameplayQuestViewModelFilter)
			{
				ref var uiGameplayQuestView = ref _uiGameplayQuestViewModelFilter.Get1(i);
				int treeId = completeQuestStepRequest.treeId;

				uiGameplayQuestView.uiGameplayQuestView.PlayQuestCompletedAnimation();
			}
		}
	}
}