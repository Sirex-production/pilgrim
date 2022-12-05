using Ingame.Input;
using Ingame.Player;
using Leopotam.Ecs;

namespace Ingame.Player
{
    public sealed class PlayerSpeedChangerSystem : IEcsRunSystem
    {
        private readonly EcsFilter<PlayerModel> _playerModelFilter;

        public void Run()
        {
            foreach (var i in _playerModelFilter)
            {
                ref var playerModel = ref _playerModelFilter.Get1(i);
                var playerData = playerModel.playerMovementData;

                float actualPlayerSpeed = playerData.WalkSpeed;

                if (playerModel.isCrouching && playerData.CrouchWalkSpeed < actualPlayerSpeed)
                    actualPlayerSpeed = playerData.CrouchWalkSpeed;

                if (playerModel.currentLeanDirection != LeanDirection.None && playerData.LeanWalkSpeed < actualPlayerSpeed)
                    actualPlayerSpeed = playerData.LeanWalkSpeed;

                playerModel.currentSpeed = actualPlayerSpeed;
            }
        }
    }
}