using Ingame.Audio;
using NaughtyAttributes;
using Support;
using Support.Extensions;
using UnityEngine;
using Zenject;

namespace Ingame.Quests.QuestSpecific
{
	public sealed class StartTheBoatWhenInteractedWithItem : PerformActionWhenInteractedWithItem
	{
		[BoxGroup("References"), Required, SerializeField] private ParticleSystem engineSmokeParticleSystem;
		
		[BoxGroup("Settings"), SerializeField] [Min(0)] private int delayInSecondsBeforeLoadingNextScene = 3;
		[BoxGroup("Settings"), SerializeField] [Scene] private int sceneIndexToLoad;

		private AudioService _audioService;
		private LevelManagementService _levelManagementService;
		
		[Inject]
		private void Construct(AudioService audioService, LevelManagementService levelManagementService)
		{
			_audioService = audioService;
			_levelManagementService = levelManagementService;
		}
		
		public override void OnInteract()
		{
			engineSmokeParticleSystem.Play();
			_audioService.Play3D("environment", "engine", transform, false);

			this.WaitAndDoCoroutine(delayInSecondsBeforeLoadingNextScene, () => _levelManagementService.LoadLevel(sceneIndexToLoad));
		}
	}
}