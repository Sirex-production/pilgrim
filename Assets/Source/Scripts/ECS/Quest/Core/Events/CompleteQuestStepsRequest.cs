using Support;

namespace Ingame.Quests
{
	public struct CompleteQuestStepsRequest
	{
		public int questId;
		public Bitset32 stepsToComplete;
	}
}