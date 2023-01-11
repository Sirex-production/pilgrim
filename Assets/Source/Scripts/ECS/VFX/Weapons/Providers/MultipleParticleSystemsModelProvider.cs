using UnityEngine;
using Voody.UniLeo;
using Zenject;

namespace Ingame.VFX
{
	public sealed class MultipleParticleSystemsModelProvider : MonoProvider<MultipleParticleSystemsModel>
	{
		[SerializeField] private ParticleSystem[] particleSystems;

		[Inject]
		private void Construct()
		{
			value = new MultipleParticleSystemsModel
			{
				particleSystems = particleSystems
			};
		}
	}
}