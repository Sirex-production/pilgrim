using System.Runtime.CompilerServices;
using Ingame.Hud;
using Ingame.Input;
using Ingame.Utils;
using Leopotam.Ecs;

namespace Ingame.Gunplay
{
    public sealed class RifleShootSystem : IEcsRunSystem
    {
        private readonly EcsFilter<FirearmComponent, RifleComponent, TimerComponent, InInventoryTag, HudIsInHandsTag, BulletIsInChamberTag> _rifleShootFilter;
        private readonly EcsFilter<ShootInputEvent> _shootInputFilter;
        
        public void Run()
        {
            if(_shootInputFilter.IsEmpty())
                return;

            foreach (var i in _rifleShootFilter)
            {
                ref var rifleEntity = ref _rifleShootFilter.GetEntity(i);
                ref var rifleComponent = ref _rifleShootFilter.Get2(i);
                ref var timerComponent = ref _rifleShootFilter.Get3(i);

                if (timerComponent.timePassed < rifleComponent.rifleConfig.PauseBetweenShots)
                    continue;
                
                rifleEntity.Get<AwaitingShotTag>();
                timerComponent.timePassed = 0;
                
                TryToBringBulletToTheChamberFromMagazine(rifleEntity);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void TryToBringBulletToTheChamberFromMagazine(in EcsEntity rifleEntity)
        {
            if(!rifleEntity.Has<MagazineComponent>())
                return;

            ref var magazineComponent = ref rifleEntity.Get<MagazineComponent>();

            if (magazineComponent.currentAmountOfAmmo < 1)
            {
                rifleEntity.Del<BulletIsInChamberTag>();
                
                TryToPutShutterInDelayPosition(rifleEntity);
                return;
            }

            magazineComponent.currentAmountOfAmmo--;
            // rifleEntity.Get<BulletIsInChamberTag>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void TryToPutShutterInDelayPosition(in EcsEntity rifleEntity)
        {
            if(!rifleEntity.Has<Ar15Tag>() && !rifleEntity.Has<M14EbrTag>())
                return;

            rifleEntity.Get<ShutterIsInDelayPositionTag>();
        }
    }
}