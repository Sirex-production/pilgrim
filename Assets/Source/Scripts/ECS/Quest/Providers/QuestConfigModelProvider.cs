using NaughtyAttributes;
using UnityEngine;
using Voody.UniLeo;
using Zenject;

namespace Ingame.Quests
{
	public sealed class QuestConfigModelProvider : MonoProvider<QuestConfigModel>
	{
		[Required, SerializeField] private QuestsConfig questsConfig;

		[Inject]
		private void Construct()
		{
			value = new QuestConfigModel
			{
				questsConfig = questsConfig
			};
		}
	}
}