using Ingame.Input;
using Leopotam.Ecs;

namespace Ingame.UI.Pause
{
	public sealed class OpenHidePauseMenuSystem : IEcsRunSystem
	{
		private readonly EcsFilter<PauseMenuServiceModel> _pauseMenuServiceFilter;
		private readonly EcsFilter<PauseInputEvent> _openPauseMenuInputEvent;

		public void Run()
		{
			if(_openPauseMenuInputEvent.IsEmpty())
				return;

			foreach (var i in _pauseMenuServiceFilter)
			{
				var pauseMenuService = _pauseMenuServiceFilter.Get1(i).pauseMenuService;

				if(pauseMenuService.IsOpened)
					pauseMenuService.RequestToHidePauseMenu();
				else
					pauseMenuService.RequestToShowPauseMenu();
			}
		}
	}
}