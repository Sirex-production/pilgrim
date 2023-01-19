using UnityEngine;

namespace Ingame.Quests.QuestSpecific
{
	public abstract class PerformActionWhenInteractedWithItem : MonoBehaviour
	{
		public abstract void OnInteract();
	}
}