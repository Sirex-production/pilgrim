using Ingame.QuestInventory;

namespace Ingame.Quests.QuestSpecific
{
	public struct OpenWithItemComponent
	{
		public PickableItemConfig requiredItemToOpen;
		public bool isOpenedOnce;
		public string openAnimationTriggerName;
	}
}