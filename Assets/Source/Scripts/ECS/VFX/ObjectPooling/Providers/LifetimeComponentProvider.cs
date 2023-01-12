using UnityEngine;
using Voody.UniLeo;
using Zenject;

namespace Ingame.VFX.Pooling
{
	public sealed class LifetimeComponentProvider : MonoProvider<LifetimeComponent>
	{
		[SerializeField] [Min(0f)] private float initialLifetime;

		[Inject]
		private void Construct()
		{
			value = new LifetimeComponent
			{
				lifetime = initialLifetime
			};
		}
	}
}