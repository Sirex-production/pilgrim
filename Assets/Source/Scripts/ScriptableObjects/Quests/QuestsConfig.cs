using System;
using UnityEngine;

namespace Ingame.Quests
{
	public sealed class QuestsConfig : ScriptableObject
	{
		private QuestTree[] _questTrees;

		public string GetQuestTreeTitle(int treeId)
		{
			return _questTrees[treeId].title;
		}

		public int GetAmountOfStepsForTree(int treeId)
		{
			return _questTrees[treeId].quests.Length;
		}

		public QuestStep GetQuestStep(in QuestDefinition questDefinition)
		{
			return _questTrees[questDefinition.treeId].quests[questDefinition.stepId];
		}
		
		public QuestStep GetQuestStep(int treeId, int stepId)
		{
			return _questTrees[treeId].quests[stepId];
		}
	}

	public struct QuestDefinition : IEquatable<QuestDefinition>
	{
		public int treeId;
		public int stepId;
		
		public bool Equals(QuestDefinition other)
		{
			return treeId == other.treeId && stepId == other.stepId;
		}
		
		public static bool operator ==(QuestDefinition def1, QuestDefinition def2)
		{
			return def1.Equals(def2);
		}

		public static bool operator !=(QuestDefinition def1, QuestDefinition def2)
		{
			return !def1.Equals(def2);
		}
	}

	[Serializable]
	public class QuestTree
	{
		public string title;
		public QuestStep[] quests;
	}

	[Serializable]
	public struct QuestStep
	{
		public string title;
		public string description;
	}
}