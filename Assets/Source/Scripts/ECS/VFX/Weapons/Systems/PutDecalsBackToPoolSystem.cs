using Ingame.Movement;
using Ingame.Utils;
using Ingame.VFX.Pooling;
using Leopotam.Ecs;
using Support;
using UnityEngine;

namespace Ingame.VFX
{
	public sealed class PutDecalsBackToPoolSystem : IEcsRunSystem
	{
		private readonly EcsFilter<TimerComponent, LifetimeComponent, DecalComponent, TransformModel> _decalFilter;
		private readonly EcsFilter<ObjectPoolComponent, BulletDecalsObjectPoolTag> _bulletDecalFilter;

		public void Run()
		{
			if(_bulletDecalFilter.IsEmpty())
				return;

			ref var bulletDecalsObjectPoolComponent = ref _bulletDecalFilter.Get1(0);

			foreach (var i in _decalFilter)
			{
				ref var decalEntity = ref _decalFilter.GetEntity(i);
				ref var timerComp = ref _decalFilter.Get1(i);
				ref var lifetimeComp = ref _decalFilter.Get2(i);
				ref var transformModel = ref _decalFilter.Get4(i);
				
				if (!transformModel.transform.TryGetComponent(out EntityReference entityReference))
				{
					TemplateUtils.SafeDebug($"Decal pooling object does not have {nameof(EntityReference)} component attached");
					
					decalEntity.Destroy();
					Object.Destroy(transformModel.transform.gameObject);

					return;
				}
				
				if(lifetimeComp.lifetime <= timerComp.timePassed)
					bulletDecalsObjectPoolComponent.objectPool.Release(entityReference);
			}
		}
	}
}