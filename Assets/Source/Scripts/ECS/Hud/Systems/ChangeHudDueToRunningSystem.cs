using Ingame.Animation;
using Ingame.Movement;
using Ingame.Player;
using Leopotam.Ecs;

namespace Ingame.Hud
{
	public sealed class ChangeHudDueToRunningSystem : IEcsRunSystem
	{
		private readonly EcsFilter<HudPlayerItemContainerComponent, AnimatorModel> _hudPlayerItemContainerFilter;
		private readonly EcsFilter<PlayerModel, VelocityComponent> _playerFilter;
		
		public void Run()
		{
			if(_playerFilter.IsEmpty() || _hudPlayerItemContainerFilter.IsEmpty())
				return;
			
			ref var playerMdl = ref _playerFilter.Get1(0);
			ref var hudAnimatorMdl = ref _hudPlayerItemContainerFilter.Get2(0);

			bool isMoving = _playerFilter.Get2(0).velocity.sqrMagnitude > 1f;
			bool isRunning = playerMdl is { isRunning: true, isCrouching: false };
			
			hudAnimatorMdl.animator.SetBool("IsRunning", isRunning && isMoving);
		}
	}
}