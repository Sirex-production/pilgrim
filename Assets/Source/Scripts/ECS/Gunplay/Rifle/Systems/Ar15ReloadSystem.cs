using System.Runtime.CompilerServices;
using Ingame.Animation;
using Ingame.Hud;
using Ingame.Inventory;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Gunplay
{
	public sealed class Ar15ReloadSystem : IEcsRunSystem
	{
		private EcsWorld _world;
		
		private readonly EcsFilter<MagazineComponent, Ar15Tag, HudIsInHandsTag> _ar15Filter;
		private readonly EcsFilter<AmmoBoxComponent> _ammoBoxFilter;

		private readonly EcsFilter<PerformDistortShutterAnimationCallbackEvent> _distortShutterAnimFilter;
		private readonly EcsFilter<PerformShutterDelayAnimationCallbackEvent> _shutterDelayAnimFilter;
		private readonly EcsFilter<PerformMagazineSwitchAnimationCallback> _magazineSwitchAnimFilter;

		public void Run()
		{
			bool isShutterDistortion = !_distortShutterAnimFilter.IsEmpty(); 
			bool isShutterDelay = !_shutterDelayAnimFilter.IsEmpty(); 
			bool isMagSwitch = !_magazineSwitchAnimFilter.IsEmpty(); 
			
			if(!isShutterDistortion && !isShutterDelay && !isMagSwitch)
				return;
			
			foreach (var i in _ar15Filter)
			{
				ref var ar15Entity = ref _ar15Filter.GetEntity(i);
				
				if(isShutterDistortion)
					TryPerformShutterDistortion(ar15Entity);
				
				if(isShutterDelay)
					TryPerformShutterDelay(ar15Entity);
				
				if(isMagSwitch)
					TryPerformMagSwitch(ar15Entity);
			}

			_world.NewEntity().Get<UpdateBackpackAppearanceEvent>();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void TryPerformShutterDistortion(in EcsEntity ar15Entity)
		{
			if(!ar15Entity.Has<AwaitsShutterDistortionTag>())
				return;
			
			ar15Entity.Del<AwaitsShutterDistortionTag>();
			
			if(ar15Entity.Has<ShutterIsInDelayPositionTag>())
				ar15Entity.Del<ShutterIsInDelayPositionTag>();
			
			if(!ar15Entity.Has<MagazineComponent>())
				return;

			ref var magazineComponent = ref ar15Entity.Get<MagazineComponent>();
			
			if(magazineComponent.currentAmountOfAmmo < 1)
				return;

			magazineComponent.currentAmountOfAmmo--;
			ar15Entity.Get<BulletIsInChamberTag>();
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void TryPerformShutterDelay(in EcsEntity ar15Entity)
		{
			if(!ar15Entity.Has<AwaitsShutterDelayTag>())
				return;
			
			ar15Entity.Del<AwaitsShutterDelayTag>();
			
			if(!ar15Entity.Has<ShutterIsInDelayPositionTag>())
				return;
			
			ar15Entity.Del<ShutterIsInDelayPositionTag>();
			
			if(!ar15Entity.Has<MagazineComponent>())
				return;
			
			ref var magazineComponent = ref ar15Entity.Get<MagazineComponent>();
			
			if(magazineComponent.currentAmountOfAmmo < 1)
				return;

			magazineComponent.currentAmountOfAmmo--;
			ar15Entity.Get<BulletIsInChamberTag>();
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void TryPerformMagSwitch(in EcsEntity ar15Entity)
		{
			if(!ar15Entity.Has<AwaitsMagazineSwitchTag>())
				return;
			
			ar15Entity.Del<AwaitsMagazineSwitchTag>();
			
			if(!ar15Entity.Has<MagazineComponent>() || _ar15Filter.IsEmpty())
				return;
			
			ref var magazineComponent = ref ar15Entity.Get<MagazineComponent>();
			ref var ammoBox = ref _ammoBoxFilter.Get1(0);

			int ammoTypeIndex = (int) magazineComponent.ammoType;
			int amountOfAmmoRequired = magazineComponent.maximumAmmoCapacity - magazineComponent.currentAmountOfAmmo;
			int ammoCanBeTakenFormAmmoBox = Mathf.Min(amountOfAmmoRequired, ammoBox.ammo[ammoTypeIndex]);

			magazineComponent.currentAmountOfAmmo += ammoCanBeTakenFormAmmoBox;
			ammoBox.ammo[ammoTypeIndex] -= ammoCanBeTakenFormAmmoBox;
		}
	}
}