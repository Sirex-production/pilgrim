using System;
using UnityEngine;

namespace Ingame.Quests
{
	[CreateAssetMenu(menuName = "Ingame/QuestConfig")]
	public sealed class QuestsConfig : ScriptableObject
	{
		[SerializeField] private QuestTree[] questTrees;
		[SerializeField] private StringIntDictionary nameAliesToTreeIndex;

		public int GetTreeId(string aliesName)
		{
			if (!nameAliesToTreeIndex.ContainsKey(aliesName))
				return -1;
			
			return nameAliesToTreeIndex[aliesName];
		}

		public int GetStepsCount(string aliasName)
		{
			int treeId = nameAliesToTreeIndex[aliasName];
			
			return questTrees[treeId].questSteps.Length;
		}
		
		public int GetStepsCount(int treeId)
		{
			return questTrees[treeId].questSteps.Length;
		}

		public QuestStep GetQuestStep(string aliasName, int stepId)
		{
			int treeId = nameAliesToTreeIndex[aliasName];

			return questTrees[treeId].questSteps[stepId];
		}

		public QuestStep GetQuestStep(int treeId, int stepId)
		{
			return questTrees[treeId].questSteps[stepId];
		}
	}

	[Serializable]
	public struct QuestTree
	{
		public QuestStep[] questSteps;
	}

	[Serializable]
	public struct QuestStep
	{
		public string title;
		public string description;
	}

	[Serializable]
	public class StringIntDictionary : SerializableDictionary<string, int> { }
}