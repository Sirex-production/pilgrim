using Ingame.Input;
using Leopotam.Ecs;

namespace Ingame.UI
{
	public sealed class DisplayQuestInfoSystem : IEcsRunSystem
	{
		private readonly EcsFilter<ShowQuestsInputEvent> _showQuests;
		private readonly EcsFilter<UiGameplayQuestViewModel> _uiQuestViewFilter;

		public void Run()
		{
			if(_showQuests.IsEmpty())
				return;
			
			foreach (var i in _uiQuestViewFilter)
			{
				var questView = _uiQuestViewFilter.Get1(i);
				
				questView.uiGameplayQuestView.DisplayTemporaryQuestInfo();
			}
		}
	}
}