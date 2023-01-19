using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Ingame.Animation;
using Ingame.Interaction.Common;
using Ingame.QuestInventory;
using Leopotam.Ecs;

namespace Ingame.Quests.QuestSpecific
{
	public sealed class OpenWithItemSystem : IEcsRunSystem
	{
		private readonly EcsFilter<OpenWithItemComponent, AnimatorModel, PerformInteractionTag> _openWithItemFilter;
		private readonly EcsFilter<ItemModel, PickedUpItemTag> _itemsInInventoryFilter;

		private readonly HashSet<PickableItemConfig> _inventoryItemsHashSet = new(16);

		public void Run()
		{
			if(_openWithItemFilter.IsEmpty())
				return;

			CollectInventoryItems();
			
			foreach (var i in _openWithItemFilter)
			{
				ref var openWithItemEntity = ref _openWithItemFilter.GetEntity(i);
				ref var openWithItemComp = ref _openWithItemFilter.Get1(i);
				ref var animatorModel = ref _openWithItemFilter.Get2(i);

				openWithItemEntity.Del<PerformInteractionTag>();

				if (_inventoryItemsHashSet.Contains(openWithItemComp.requiredItemToOpen))
				{
					animatorModel.animator.ResetTrigger(openWithItemComp.openAnimationTriggerName);
					animatorModel.animator.SetTrigger(openWithItemComp.openAnimationTriggerName);
					
					if(openWithItemComp.isOpenedOnce)
						openWithItemEntity.Del<InteractiveTag>();
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void CollectInventoryItems()
		{
			_inventoryItemsHashSet.Clear();

			foreach (var i in _itemsInInventoryFilter)
				_inventoryItemsHashSet.Add(_itemsInInventoryFilter.Get1(i).itemConfig);
		}
	}
}