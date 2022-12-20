using System;
using System.Linq;
using UnityEngine;

namespace Ingame.Quests
{
	[CreateAssetMenu(menuName = "Ingame/QuestConfig")]
	public sealed class QuestsConfig : ScriptableObject
	{
		[SerializeField] private Quest[] quests;
		[SerializeField] private StringIntDictionary nameAliesToTreeIndex;

		public string[] GetQuestStepDescriptions(string alies)
		{
			return GetQuestStepDescriptions(nameAliesToTreeIndex[alies]);
		}
		
		public string[] GetQuestStepDescriptions(int questId)
		{
			return quests[questId].steps.Select(step => step.description).ToArray();
		}

		public string GetQuestTitle(string alies)
		{
			return GetQuestTitle(nameAliesToTreeIndex[alies]);
		}
		
		public string GetQuestTitle(int questId)
		{
			return quests[questId].title;
		}

		public int GetStepsCount(string alies)
		{
			return GetStepsCount(nameAliesToTreeIndex[alies]);
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
	}

	[Serializable]
	public class StringIntDictionary : SerializableDictionary<string, int> { }
}