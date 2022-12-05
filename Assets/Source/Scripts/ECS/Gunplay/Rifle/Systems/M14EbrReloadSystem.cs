using System.Runtime.CompilerServices;
using Ingame.Animation;
using Ingame.Hud;
using Ingame.Inventory;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Gunplay
{
	public sealed class M14EbrReloadSystem : IEcsRunSystem
	{
		private EcsWorld _world;
		
		private readonly EcsFilter<MagazineComponent, M14EbrTag, HudIsInHandsTag> _m14EbrFilter;
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
			
			foreach (var i in _m14EbrFilter)
			{
				ref var m14EbrEntity = ref _m14EbrFilter.GetEntity(i);
				
				if(isShutterDistortion)
					TryPerformShutterDistortion(m14EbrEntity);
				
				if(isShutterDelay)
					TryPerformShutterDelay(m14EbrEntity);
				
				if(isMagSwitch)
					TryPerformMagSwitch(m14EbrEntity);
			}

			_world.NewEntity().Get<UpdateBackpackAppearanceEvent>();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void TryPerformShutterDistortion(in EcsEntity m14EbrEntity)
		{
			if(!m14EbrEntity.Has<AwaitsShutterDistortionTag>())
				return;
			
			m14EbrEntity.Del<AwaitsShutterDistortionTag>();
			
			if(m14EbrEntity.Has<ShutterIsInDelayPositionTag>())
				m14EbrEntity.Del<ShutterIsInDelayPositionTag>();
			
			if(!m14EbrEntity.Has<MagazineComponent>())
				return;

			ref var magazineComponent = ref m14EbrEntity.Get<MagazineComponent>();
			
			if(magazineComponent.currentAmountOfAmmo < 1)
				return;

			magazineComponent.currentAmountOfAmmo--;
			m14EbrEntity.Get<BulletIsInChamberTag>();
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void TryPerformShutterDelay(in EcsEntity m14EbrEntity)
		{
			if(!m14EbrEntity.Has<AwaitsShutterDelayTag>())
				return;
			
			m14EbrEntity.Del<AwaitsShutterDelayTag>();
			
			if(!m14EbrEntity.Has<ShutterIsInDelayPositionTag>())
				return;
			
			m14EbrEntity.Del<ShutterIsInDelayPositionTag>();
			
			if(!m14EbrEntity.Has<MagazineComponent>())
				return;
			
			ref var magazineComponent = ref m14EbrEntity.Get<MagazineComponent>();
			
			if(magazineComponent.currentAmountOfAmmo < 1)
				return;

			magazineComponent.currentAmountOfAmmo--;
			m14EbrEntity.Get<BulletIsInChamberTag>();
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void TryPerformMagSwitch(in EcsEntity m14EbrEntity)
		{
			if(!m14EbrEntity.Has<AwaitsMagazineSwitchTag>())
				return;
			
			m14EbrEntity.Del<AwaitsMagazineSwitchTag>();
			
			if(!m14EbrEntity.Has<MagazineComponent>() || _m14EbrFilter.IsEmpty())
				return;
			
			ref var magazineComponent = ref m14EbrEntity.Get<MagazineComponent>();
			ref var ammoBox = ref _ammoBoxFilter.Get1(0);

			int ammoTypeIndex = (int) magazineComponent.ammoType;
			int amountOfAmmoRequired = magazineComponent.maximumAmmoCapacity - magazineComponent.currentAmountOfAmmo;
			int ammoCanBeTakenFormAmmoBox = Mathf.Min(amountOfAmmoRequired, ammoBox.ammo[ammoTypeIndex]);

			magazineComponent.currentAmountOfAmmo += ammoCanBeTakenFormAmmoBox;
			ammoBox.ammo[ammoTypeIndex] -= ammoCanBeTakenFormAmmoBox;
		}
	}
}