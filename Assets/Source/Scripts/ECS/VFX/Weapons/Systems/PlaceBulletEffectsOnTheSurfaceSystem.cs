using Ingame.ConfigProvision;
using Ingame.VFX.Pooling;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Ingame.VFX
{
	public sealed class PlaceBulletEffectsOnTheSurfaceSystem : IEcsRunSystem
	{
		private readonly ConfigProviderService _configProviderService;
		
		private readonly EcsFilter<PlaceBulletVfxRequest> _placeBulletVfxRequestFilter;
		private readonly EcsFilter<ObjectPoolComponent, BulletDecalsObjectPoolTag> _bulletDecalObjectPoolComp;

		public void Run()
		{
			if(_bulletDecalObjectPoolComp.IsEmpty())
				return;
			
			ref var bulletDecalsObjectPoolComponent = ref _bulletDecalObjectPoolComp.Get1(0);

			foreach (var i in _placeBulletVfxRequestFilter)
			{
				ref var placeVfxReqEntity = ref _placeBulletVfxRequestFilter.GetEntity(i);
				ref var placeVfxReq = ref _placeBulletVfxRequestFilter.Get1(i);
				var decalEntityRef = bulletDecalsObjectPoolComponent.objectPool.Get();
				var decalTransform = decalEntityRef.transform;

				if(decalEntityRef == null)
					return;
				
				decalTransform.position = placeVfxReq.position;
				decalTransform.localRotation = Quaternion.LookRotation(placeVfxReq.surfaceNormalDirection);

				if (decalEntityRef.TryGetComponent(out DecalProjector decalProjector))
				{
					decalProjector.material = _configProviderService
						.VFXSurfaceTypeConfig
						.GetVfxSurfaceDataDueToSurfaceTag(placeVfxReq.surfaceTag)
						.RandomBulletHoleMaterial;
				}

				placeVfxReqEntity.Del<PlaceBulletVfxRequest>();
			}
		}
	}
}