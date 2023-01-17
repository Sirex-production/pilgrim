using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Voody.UniLeo;
using Zenject;

namespace Ingame.VFX
{
	public sealed class DecalComponentProvider : MonoProvider<DecalComponent>
	{
		[Required, SerializeField] private DecalProjector decalProjector;

		[Inject]
		private void Construct()
		{
			value = new DecalComponent
			{
				decalProjector = decalProjector
			};
		}
	}
}