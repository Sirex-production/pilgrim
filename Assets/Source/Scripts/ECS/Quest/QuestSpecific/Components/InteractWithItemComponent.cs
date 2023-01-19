using Ingame.QuestInventory;

namespace Ingame.Quests.QuestSpecific
{
	public struct InteractWithItemComponent
	{
		public PerformActionWhenInteractedWithItem performActionWhenInteractedWithItem;
		public PickableItemConfig requiredItem;
	}
}