using Ingame.Input;
using Leopotam.Ecs;

namespace Ingame.Player
{
	public sealed class PlayerInputToRunConverterSystem : IEcsRunSystem
	{
		private readonly EcsFilter<PlayerModel> _playerFilter;
		private readonly EcsFilter<RunInputEvent> _runInputEventFilter;
		
		public void Run()
		{
			if(_playerFilter.IsEmpty())
				return;

			ref var playerMdl = ref _playerFilter.Get1(0);
			playerMdl.isRunning = !_runInputEventFilter.IsEmpty();
		}
	}
}