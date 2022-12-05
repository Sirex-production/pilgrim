using Leopotam.Ecs;
using UnityEngine;
using Zenject;

namespace Ingame.Animation
{
	public sealed class ItemsContainerAnimationCallbacks : MonoBehaviour
	{
		[Inject] private readonly EcsWorld _world;

		public void PerformUpdateItemsVisibilityCallback()
		{
			_world.NewEntity().Get<UpdateItemVisibilityAnimationCallbackEvent>();
		}
	}
}