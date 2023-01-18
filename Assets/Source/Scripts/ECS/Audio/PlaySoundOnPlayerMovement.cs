using Ingame.Audio;
using Ingame.Movement;
using Ingame.Player;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Audio
{

    public sealed class PlaySoundOnPlayerMovement : IEcsRunSystem
    {
        private static readonly float STEP_INTERVAL = 0.65f;
        private readonly EcsFilter<VelocityComponent, CharacterControllerModel, PlayerModel>.Exclude<BlockMovementRequest> _velocityFilter;
        private AudioService _audioService;
        
        private float _currentInterval = 0;
        public void Run()
        {
            if (_velocityFilter.IsEmpty())
            {
                return;
            }
            
            ref var velocityComponent = ref _velocityFilter.Get1(0);

            if (velocityComponent.velocity.magnitude <= 1)
            {
                _currentInterval = STEP_INTERVAL;
                return;
            }

            _currentInterval += Time.deltaTime;
            
            if (!(_currentInterval > STEP_INTERVAL)) 
                return;
            
            _audioService.Play("player","walk" ,true);
            _currentInterval = 0;
        }
    }
}