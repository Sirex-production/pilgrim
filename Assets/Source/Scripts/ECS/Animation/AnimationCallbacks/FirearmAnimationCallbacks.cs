using Leopotam.Ecs;
using UnityEngine;
using Zenject;

namespace Ingame.Animation
{
	public sealed class FirearmAnimationCallbacks : MonoBehaviour
	{
		[Inject] private readonly EcsWorld _world; 
		
		public void PerformDistortTheShutterCallback()
		{
			_world.NewEntity().Get<PerformDistortShutterAnimationCallbackEvent>();
		}

		public void PerformShutterDelayCallback()
		{
			_world.NewEntity().Get<PerformShutterDelayAnimationCallbackEvent>();
		}

		public void PerformMagazineSwitchDelayCallback()
		{
			_world.NewEntity().Get<PerformMagazineSwitchAnimationCallback>();
		}
	}
}