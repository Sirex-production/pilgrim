using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Ingame.Interaction.Common;
using Ingame.QuestInventory;
using Leopotam.Ecs;

namespace Ingame.Quests.QuestSpecific
{
	public sealed class PerformInteractionWithItemSystem : IEcsRunSystem
	{
		private readonly EcsFilter<InteractWithItemComponent, PerformInteractionTag> _performInteractionWithItemFilter;
		private readonly EcsFilter<ItemModel, PickedUpItemTag> _itemsInInventoryFilter;
		
		private readonly HashSet<PickableItemConfig> _inventoryItemsHashSet = new(16);

		public void Run()
		{
			if(_itemsInInventoryFilter.IsEmpty())
				return;
			
			CollectInventoryItems();
			
			foreach (var i in _performInteractionWithItemFilter)
			{
				ref var interactWithItemEntity = ref _performInteractionWithItemFilter.GetEntity(i);
				ref var interactWithItemComponent = ref _performInteractionWithItemFilter.Get1(i);
				
				interactWithItemEntity.Del<PerformInteractionTag>();
				
				if (!_inventoryItemsHashSet.Contains(interactWithItemComponent.requiredItem))
					continue;
				
				interactWithItemComponent.performActionWhenInteractedWithItem.OnInteract();
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