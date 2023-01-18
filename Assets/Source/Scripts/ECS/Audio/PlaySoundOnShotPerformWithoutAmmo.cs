using Ingame.Gunplay;
using Ingame.Hud;
using Ingame.Input;
using Ingame.Utils;
using Leopotam.Ecs;

namespace Ingame.Audio
{
    public sealed class PlaySoundOnShotPerformWithoutAmmo : IEcsRunSystem
    {
        private const float PAUSE_RATE = 4.25f;
        private readonly AudioService _audioService;
        private readonly EcsFilter<ShootInputEvent> _shootInputFilter;
        private readonly EcsFilter<MagazineComponent,FirearmComponent, RifleComponent, TimerComponent, InInventoryTag, HudIsInHandsTag> _rifleShootFilter;
        public void Run()
        {
             if(_shootInputFilter.IsEmpty())
                 return;

             if(_rifleShootFilter.IsEmpty())
                 return;
             
             ref var entity = ref _rifleShootFilter.GetEntity(0);
             ref var magazineComponent = ref _rifleShootFilter.Get1(0);
             ref var firearmComponent= ref _rifleShootFilter.Get2(0);
             ref var rifleComponent = ref _rifleShootFilter.Get3(0);
             ref var timerComponent = ref entity.Get<TimerComponent>();
             
             if(magazineComponent.currentAmountOfAmmo>0)
                 return;
             
             if (timerComponent.timePassed < rifleComponent.rifleConfig.PauseBetweenShots*PAUSE_RATE)
                 return;
             
             timerComponent.timePassed = 0;
             _audioService.Play3D("player","empty", firearmComponent.barrelOrigin);
        }
    }
}