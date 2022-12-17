using Leopotam.Ecs;
using NaughtyAttributes;
using Support;
using UnityEngine;
using Zenject;

namespace Ingame.Quests
{
	[RequireComponent(typeof(Collider))]
	public sealed class CompleteQuestStepTrigger : MonoBehaviour
	{
		[Required, SerializeField] private QuestsConfig questsConfig;

		[SerializeField] private bool isTreeAliesNameUsed = false;
		[ShowIf(nameof(isTreeAliesNameUsed))]
		[SerializeField] private string treeAliesName;
		[HideIf(nameof(isTreeAliesNameUsed))]
		[SerializeField] private int treeId;
		[SerializeField] private int questStepId;

		private EcsWorld _world;
		
		[Inject]
		private void Construct(EcsWorld world)
		{
			_world = world;
		}

		private void OnTriggerEnter(Collider other)
		{
			int targetTreeId = isTreeAliesNameUsed ? questsConfig.GetTreeId(treeAliesName) : treeId;

			if (targetTreeId < 0)
			{
				TemplateUtils.SafeDebug("Incorrect tree alies name is specified");
				return;
			}

			ref var completeStepReq = ref _world.NewEntity().Get<CompleteQuestStepRequest>();
			completeStepReq.treeId = targetTreeId;
		}
	}
}