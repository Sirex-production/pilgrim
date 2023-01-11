using Ingame.Gunplay;
using Leopotam.Ecs;

namespace Ingame.VFX
{
	public sealed class PlayMuzzleFlashEffectSystem : IEcsRunSystem
	{
		private readonly EcsFilter<MultipleParticleSystemsModel, AwaitingShotTag> _shootingWeaponFilter;

		public void Run()
		{
			foreach (var i in _shootingWeaponFilter)
			{
				ref var particles = ref _shootingWeaponFilter.Get1(i);
				
				foreach (var particleSystem in particles.particleSystems) 
					particleSystem.Play();
			}
		}
	}
}