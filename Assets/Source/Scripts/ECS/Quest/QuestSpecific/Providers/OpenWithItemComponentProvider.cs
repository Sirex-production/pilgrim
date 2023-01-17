using Ingame.QuestInventory;
using NaughtyAttributes;
using UnityEngine;
using Voody.UniLeo;
using Zenject;

namespace Ingame.Quests.QuestSpecific
{
	public sealed class OpenWithItemComponentProvider : MonoProvider<OpenWithItemComponent>
	{
		[Required, SerializeField] private PickableItemConfig requiredItemToOpen;
		[SerializeField] private bool isOpenedOnce;
		[SerializeField] private string openAnimationName;

		[Inject]
		private void Construct()
		{
			value = new OpenWithItemComponent
			{
				requiredItemToOpen = requiredItemToOpen,
				isOpenedOnce = isOpenedOnce,
				openAnimationTriggerName = openAnimationName
			};
		}
	}
}