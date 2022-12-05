using Ingame.Hud;
using Ingame.Movement;
using Leopotam.Ecs;
using Support.Extensions;

namespace Ingame.Animation
{
	public sealed class ShowHideHudItemSystem : IEcsRunSystem
	{
		private readonly EcsFilter<HudItemModel, TransformModel, InInventoryTag> _hudItemsFilter;
		private readonly EcsFilter<UpdateItemVisibilityAnimationCallbackEvent> _updateItemsVisibilityCallbackFilter;

		public void Run()
		{
			if(_updateItemsVisibilityCallbackFilter.IsEmpty())
				return;

			foreach (var i in _hudItemsFilter)
			{
				ref var itemEntity = ref _hudItemsFilter.GetEntity(i);
				var itemTransform = _hudItemsFilter.Get2(i).transform;

				if (itemEntity.Has<AwaitsToBeShownTag>())
				{
					itemEntity.Get<HudIsInHandsTag>();
					itemEntity.Del<AwaitsToBeShownTag>();
					
					itemTransform.SetGameObjectActive();
				}

				if (itemEntity.Has<AwaitsToBeHiddenTag>())
				{
					if(itemEntity.Has<HudIsInHandsTag>())
						itemEntity.Del<HudIsInHandsTag>();
					itemEntity.Del<AwaitsToBeHiddenTag>();
					
					itemTransform.SetGameObjectInactive();
				}
			}
		}
	}
}