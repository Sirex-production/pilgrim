using Ingame.Input;
using Leopotam.Ecs;

namespace Ingame.UI
{
	public sealed class DisplayQuestInfoSystem : IEcsRunSystem
	{
		private readonly EcsFilter<UiGameplayQuestViewModel> _questViewFilter;
		private readonly EcsFilter<ShowActiveQuestInputEvent> _showCurrentQuestInputFilter;
		private readonly EcsFilter<ShowAllQuestsInputEvent> _showAllQuestsInputFilter;

		public void Run()
		{
			bool isActiveQuestShouldBeShown = !_showCurrentQuestInputFilter.IsEmpty();
			bool areAllQuestsShouldBeShown = !_showAllQuestsInputFilter.IsEmpty();

			if(!isActiveQuestShouldBeShown && !areAllQuestsShouldBeShown)
				return;
			
			foreach (var i in _questViewFilter)
			{
				ref var questView = ref _questViewFilter.Get1(i);
			
				if(isActiveQuestShouldBeShown)
					questView.uiGameplayQuestView.TemporaryShowActiveQuest();
				
				if(areAllQuestsShouldBeShown)
					questView.uiGameplayQuestView.TemporaryShowAllQuests();
			}
		}
	}
}