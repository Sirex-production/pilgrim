using System;
using System.Linq;
using Ingame.QuestInventory;
using NaughtyAttributes;
using UnityEngine;

namespace Ingame.Quests
{
	[CreateAssetMenu(menuName = "Ingame/QuestConfig")]
	public sealed class QuestsConfig : ScriptableObject
	{
		[SerializeField] private Quest[] quests;

		public Quest GetQuest(int questId)
		{
			return quests[questId];
		}

		public string[] GetQuestStepDescriptions(int questId)
		{
			return quests[questId].steps.Select(step => step.description).ToArray();
		}

		public string GetQuestTitle(int questId)
		{
			return quests[questId].title;
		}

		public int GetStepsCount(int questId)
		{
			return quests[questId].steps.Length;
		}
	}

	[Serializable]
	public struct Quest
	{
		public string title;
		public QuestStep[] steps;
	}

	[Serializable]
	public struct QuestStep
	{
		public string description;
		[Space]
		public bool isCompletedByFindingItem;
		[AllowNesting]
		[ShowIf(nameof(isCompletedByFindingItem))]
		public PickableItemConfig item;
		
		public bool isCompletedByEnteringTrigger; 
	}

	[Serializable]
	public class StringIntDictionary : SerializableDictionary<string, int> { }
}