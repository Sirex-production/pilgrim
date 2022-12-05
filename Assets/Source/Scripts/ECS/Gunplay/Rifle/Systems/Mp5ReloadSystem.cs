using System.Runtime.CompilerServices;
using Ingame.Animation;
using Ingame.Hud;
using Ingame.Inventory;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Gunplay
{
	public class Mp5ReloadSystem : IEcsRunSystem
	{
		private EcsWorld _world;
		
		private readonly EcsFilter<MagazineComponent, Mp5Tag, HudIsInHandsTag> _mp5Filter;
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
			
			foreach (var i in _mp5Filter)
			{
				ref var mp5Entity = ref _mp5Filter.GetEntity(i);
				
				if(isShutterDistortion)
					TryPerformShutterDistortion(mp5Entity);
				
				if(isShutterDelay)
					TryPerformShutterDelay(mp5Entity);
				
				if(isMagSwitch)
					TryPerformMagSwitch(mp5Entity);
			}

			_world.NewEntity().Get<UpdateBackpackAppearanceEvent>();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void TryPerformShutterDistortion(in EcsEntity mp5Entity)
		{
			if(!mp5Entity.Has<AwaitsShutterDistortionTag>())
				return;
			
			mp5Entity.Del<AwaitsShutterDistortionTag>();
			mp5Entity.Get<ShutterIsInDelayPositionTag>();
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void TryPerformShutterDelay(in EcsEntity mp5Entity)
		{
			if(!mp5Entity.Has<AwaitsShutterDelayTag>())
				return;
			
			mp5Entity.Del<AwaitsShutterDelayTag>();
			
			if(!mp5Entity.Has<ShutterIsInDelayPositionTag>())
				return;
			
			mp5Entity.Del<ShutterIsInDelayPositionTag>();
			
			if(!mp5Entity.Has<MagazineComponent>())
				return;
			
			ref var magazineComponent = ref mp5Entity.Get<MagazineComponent>();

			if (magazineComponent.currentAmountOfAmmo < 1)
			{
				if(mp5Entity.Has<BulletIsInChamberTag>())
					mp5Entity.Del<BulletIsInChamberTag>();
				
				return;
			}

			mp5Entity.Get<BulletIsInChamberTag>();
			magazineComponent.currentAmountOfAmmo--;
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void TryPerformMagSwitch(in EcsEntity mp5Entity)
		{
			if(!mp5Entity.Has<AwaitsMagazineSwitchTag>())
				return;
			
			mp5Entity.Del<AwaitsMagazineSwitchTag>();
			
			if(!mp5Entity.Has<MagazineComponent>() || _mp5Filter.IsEmpty())
				return;
			
			ref var magazineComponent = ref mp5Entity.Get<MagazineComponent>();
			ref var ammoBox = ref _ammoBoxFilter.Get1(0);

			int ammoTypeIndex = (int) magazineComponent.ammoType;
			int amountOfAmmoRequired = magazineComponent.maximumAmmoCapacity - magazineComponent.currentAmountOfAmmo;
			int ammoCanBeTakenFormAmmoBox = Mathf.Min(amountOfAmmoRequired, ammoBox.ammo[ammoTypeIndex]);

			magazineComponent.currentAmountOfAmmo += ammoCanBeTakenFormAmmoBox;
			ammoBox.ammo[ammoTypeIndex] -= ammoCanBeTakenFormAmmoBox;
		}
	}
}