using Ingame.Utils;
using Leopotam.Ecs;
using Support.Extensions;
using UnityEngine;
using UnityEngine.Pool;
using Voody.UniLeo;
using Zenject;

namespace Ingame.VFX.Pooling
{
	public sealed class ObjectPoolComponentProvider : MonoProvider<ObjectPoolComponent>
	{
		[SerializeField] private EntityReference defaultPrefab;

		private DiContainer _diContainer;
		
		[Inject]
		private void Construct(DiContainer diContainer)
		{
			value = new ObjectPoolComponent
			{
				objectPool = CreateObjectPool()
			};

			_diContainer = diContainer;
		}

		private ObjectPool<EntityReference> CreateObjectPool()
		{
			return new ObjectPool<EntityReference>(OnCreate, OnGet, OnRelease);
		}

		private EntityReference OnCreate()
		{
			var entityRef = _diContainer.InstantiatePrefabForComponent<EntityReference>(defaultPrefab);

			return entityRef;
		}
		
		private void OnGet(EntityReference entityRef)
		{
			if(entityRef == null || entityRef.Entity == EcsEntity.Null)
				return;

			entityRef.Entity.Get<TimerComponent>();
			
			entityRef.SetGameObjectActive();
		}
		
		private void OnRelease(EntityReference entityRef)
		{
			if(entityRef == null || entityRef.Entity == EcsEntity.Null)
				return;
			
			if(entityRef.Entity.Has<TimerComponent>())
				entityRef.Entity.Del<TimerComponent>();
			
			entityRef.SetGameObjectInactive();
		}
	}
}