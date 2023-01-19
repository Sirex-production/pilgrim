using Ingame.Extensions;
using Ingame.Health;
using Ingame.Player;
using Leopotam.Ecs;

namespace Ingame.LevelManagement
{
	public sealed class CheckGameOverConditionSystem : IEcsRunSystem
	{
		private readonly EcsWorld _world;
		private readonly EcsFilter<PlayerModel, DeathTag> _deadPlayerSystem;

		private bool _wasGameOvered = false;
		
		public void Run()
		{
			if(_deadPlayerSystem.IsEmpty() || _wasGameOvered)
				return;

			_wasGameOvered = true;
			_world.SendSignal<GameOverRequest>();
		}
	}
}