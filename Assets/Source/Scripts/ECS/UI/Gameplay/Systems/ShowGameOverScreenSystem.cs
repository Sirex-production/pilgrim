using Ingame.LevelManagement;
using Leopotam.Ecs;
using Support;

namespace Ingame.UI
{
	public sealed class ShowGameOverScreenSystem : IEcsRunSystem
	{
		private readonly EcsFilter<UiGameOverScreenModel> _uiGameOverScreenFilter;
		private readonly EcsFilter<GameOverRequest> _gameOverComponentFilter;
		
		public void Run()
		{
			if(_gameOverComponentFilter.IsEmpty())
				return;
			
			foreach (var i in _uiGameOverScreenFilter)
			{
				var uiGameOverScreen = _uiGameOverScreenFilter.Get1(i).uiGameOverScreen;
				uiGameOverScreen.ShowGameOverScreen();
			}
		}
	}
}