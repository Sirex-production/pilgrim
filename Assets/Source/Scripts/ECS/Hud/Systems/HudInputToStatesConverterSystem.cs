using System.Runtime.CompilerServices;
using Ingame.Gunplay;
using Ingame.Input;
using Leopotam.Ecs;

namespace Ingame.Hud
{
    public sealed class HudInputToStatesConverterSystem : IEcsRunSystem
    {
        private readonly EcsFilter<HudItemModel, InInventoryTag> _itemModelFilter;
        private readonly EcsFilter<AimInputEvent> _aimEventFilter;
        
        private readonly EcsFilter<MagazineSwitchInputEvent> _magSwitchEventFilter;
        private readonly EcsFilter<DistortTheShutterInputEvent> _distortShutterEventFilter;
        private readonly EcsFilter<ShutterDelayInputEvent> _shutterDelayEventFilter;

        public void Run()
        {
            foreach (var i in _itemModelFilter)
            {
                ref var itemEntity = ref _itemModelFilter.GetEntity(i);
                var itemData = _itemModelFilter.Get1(i).itemData;

                if (!_aimEventFilter.IsEmpty() && itemData.CanBeUsedAsAim)
                {
                    if (itemEntity.Has<HudIsAimingTag>())
                        itemEntity.Del<HudIsAimingTag>();
                    else
                        itemEntity.Get<HudIsAimingTag>();
                }

                TryPerformFirearmActions(itemEntity);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void TryPerformFirearmActions(in EcsEntity itemEntity)
        {
            if(!itemEntity.Has<HudIsInHandsTag>())
                return;

            if(!itemEntity.Has<FirearmComponent>())
                return;
            
            if (!_magSwitchEventFilter.IsEmpty())
                itemEntity.Get<AwaitsMagazineSwitchTag>();
                
            if (!_distortShutterEventFilter.IsEmpty())
                itemEntity.Get<AwaitsShutterDistortionTag>();
                
            if (!_shutterDelayEventFilter.IsEmpty())
                itemEntity.Get<AwaitsShutterDelayTag>();
        }
    }
}