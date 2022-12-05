using Ingame.Input;
using Ingame.Movement;
using Ingame.Utils;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Player
{
    public sealed class PlayerInputToJumpConverterSystem : IEcsRunSystem
    {
        private readonly EcsFilter<PlayerModel, VelocityComponent, CharacterControllerModel, TimerComponent> _playerFilter;
        private readonly EcsFilter<JumpInputEvent> _jumpFilter;

        public void Run()
        {
            if(_jumpFilter.IsEmpty())
                return;
            
            foreach (var i in _playerFilter)
            {
                ref var playerModel = ref _playerFilter.Get1(i);
                ref var playerVelocityComp = ref _playerFilter.Get2(i);
                ref var playerCharacterControllerModel = ref _playerFilter.Get3(i);
                ref var playerJumpTimer = ref _playerFilter.Get4(i);

                var playerData = playerModel.playerMovementData;

                if(!playerCharacterControllerModel.isStandingOnFlatSurface || playerJumpTimer.timePassed < playerData.PauseBetweenJumps)
                    return;

                playerJumpTimer.timePassed = 0;
                playerCharacterControllerModel.isStandingOnFlatSurface = false;
                playerVelocityComp.velocity += Vector3.up * playerModel.playerMovementData.JumpForce;
            }
        }
    }
}