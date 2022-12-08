using System.Runtime.CompilerServices;
using Ingame.Animation;
using Ingame.Hud;
using Ingame.Inventory;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Gunplay
{
	public sealed class KrissVectorReloadSystem : IEcsRunSystem
	{
		private EcsWorld _world;
		
		private readonly EcsFilter<MagazineComponent, KrissVectorTag, HudIsInHandsTag> _krissVectorFilter;
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
			
			foreach (var i in _krissVectorFilter)
			{
				ref var krissVectorEntity = ref _krissVectorFilter.GetEntity(i);
				
				if(isShutterDistortion)
					TryPerformShutterDistortion(krissVectorEntity);
				
				if(isShutterDelay)
					TryPerformShutterDelay(krissVectorEntity);
				
				if(isMagSwitch)
					TryPerformMagSwitch(krissVectorEntity);
			}

			_world.NewEntity().Get<UpdateBackpackAppearanceEvent>();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void TryPerformShutterDistortion(in EcsEntity krissVectorEntity)
		{
			if(!krissVectorEntity.Has<AwaitsShutterDistortionTag>())
				return;
			
			krissVectorEntity.Del<AwaitsShutterDistortionTag>();
			
			if(krissVectorEntity.Has<ShutterIsInDelayPositionTag>())
				krissVectorEntity.Del<ShutterIsInDelayPositionTag>();
			
			if(!krissVectorEntity.Has<MagazineComponent>())
				return;

			ref var magazineComponent = ref krissVectorEntity.Get<MagazineComponent>();
			
			if(magazineComponent.currentAmountOfAmmo < 1)
				return;

			magazineComponent.currentAmountOfAmmo--;
			krissVectorEntity.Get<BulletIsInChamberTag>();
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void TryPerformShutterDelay(in EcsEntity krissVectorEntity)
		{
			if(!krissVectorEntity.Has<AwaitsShutterDelayTag>())
				return;
			
			krissVectorEntity.Del<AwaitsShutterDelayTag>();
			
			if(!krissVectorEntity.Has<ShutterIsInDelayPositionTag>())
				return;
			
			krissVectorEntity.Del<ShutterIsInDelayPositionTag>();
			
			if(!krissVectorEntity.Has<MagazineComponent>())
				return;
			
			ref var magazineComponent = ref krissVectorEntity.Get<MagazineComponent>();
			
			if(magazineComponent.currentAmountOfAmmo < 1)
				return;

			magazineComponent.currentAmountOfAmmo--;
			krissVectorEntity.Get<BulletIsInChamberTag>();
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void TryPerformMagSwitch(in EcsEntity krissVectorEntity)
		{
			if(!krissVectorEntity.Has<AwaitsMagazineSwitchTag>())
				return;
			
			krissVectorEntity.Del<AwaitsMagazineSwitchTag>();
			
			if(!krissVectorEntity.Has<MagazineComponent>() || _krissVectorFilter.IsEmpty())
				return;
			
			ref var magazineComponent = ref krissVectorEntity.Get<MagazineComponent>();
			ref var ammoBox = ref _ammoBoxFilter.Get1(0);

			int ammoTypeIndex = (int) magazineComponent.ammoType;
			int amountOfAmmoRequired = magazineComponent.maximumAmmoCapacity - magazineComponent.currentAmountOfAmmo;
			int ammoCanBeTakenFormAmmoBox = Mathf.Min(amountOfAmmoRequired, ammoBox.ammo[ammoTypeIndex]);

			magazineComponent.currentAmountOfAmmo += ammoCanBeTakenFormAmmoBox;
			ammoBox.ammo[ammoTypeIndex] -= ammoCanBeTakenFormAmmoBox;
		}
	}
}