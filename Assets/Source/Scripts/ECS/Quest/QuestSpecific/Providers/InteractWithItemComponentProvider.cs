using Ingame.QuestInventory;
using NaughtyAttributes;
using UnityEngine;
using Voody.UniLeo;
using Zenject;

namespace Ingame.Quests.QuestSpecific
{
	public sealed class InteractWithItemComponentProvider : MonoProvider<InteractWithItemComponent>
	{
		[Required, SerializeField] private PerformActionWhenInteractedWithItem performActionWhenInteractedWithItem;
		[Required, SerializeField] private PickableItemConfig requiredItem;

		[Inject]
		private void Construct()
		{
			value = new InteractWithItemComponent
			{
				performActionWhenInteractedWithItem = performActionWhenInteractedWithItem,
				requiredItem = requiredItem
			};
		}
	}
}