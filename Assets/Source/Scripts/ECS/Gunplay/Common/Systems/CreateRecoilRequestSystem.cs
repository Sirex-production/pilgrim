using System.Runtime.CompilerServices;
using Ingame.Hud;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Gunplay
{
	public sealed class CreateRecoilRequestSystem : IEcsRunSystem
	{
		private readonly EcsWorld _world;
		private readonly EcsFilter<FirearmComponent> _shootingFirearmFilter;
		
		public void Run()
		{
			foreach (var i in _shootingFirearmFilter)
			{
				ref var firearmEntity = ref _shootingFirearmFilter.GetEntity(i);
				ref var firearmComponent = ref _shootingFirearmFilter.Get1(i);

				var firearmConfig = firearmComponent.firearmConfig;

				if (firearmEntity.Has<AwaitingShotTag>())
				{
					var recoilBoost = firearmConfig.RecoilBoost;
					recoilBoost.x *= Random.value * GetRandomSign();

					firearmComponent.currentRecoilStrength += recoilBoost;

					ref var recoilRequest = ref _world.NewEntity().Get<RecoilRequest>();
					recoilRequest.angleStrength = firearmComponent.currentRecoilStrength;
				}
				else
				{
					firearmComponent.currentRecoilStrength = Vector2.Lerp(firearmComponent.currentRecoilStrength, Vector2.zero, firearmConfig.RecoilStabilizationSpeed * Time.deltaTime);
				}
				
				ApplyHudItemRecoil(firearmEntity);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void ApplyHudItemRecoil(in EcsEntity firearmEntity)
		{
			if (!firearmEntity.Has<HudItemRecoilComponent>() || !firearmEntity.Has<HudItemModel>())
				return;

			var itemData = firearmEntity.Get<HudItemModel>().itemData;
			ref var recoilComp = ref firearmEntity.Get<HudItemRecoilComponent>();

			recoilComp.currentRecoilPosOffsetZ = firearmEntity.Has<AwaitingShotTag>()
				? itemData.RecoilOffsetZ
				: Mathf.Lerp(recoilComp.currentRecoilPosOffsetZ, 0, itemData.RecoilStabilizationSpeed * Time.deltaTime);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private int GetRandomSign() => Random.value < .5f ? 1 : -1;
	}
}