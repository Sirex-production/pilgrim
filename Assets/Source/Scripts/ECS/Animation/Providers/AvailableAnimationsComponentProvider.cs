using NaughtyAttributes;
using UnityEngine;
using Voody.UniLeo;
using Zenject;

namespace Ingame.Animation
{
	public sealed class AvailableAnimationsComponentProvider : MonoProvider<AvailableAnimationsComponent>
	{
		[EnumFlags]
		[SerializeField] AnimationType availableAnimationTypes;

		[Inject]
		private void Construct()
		{
			value = new AvailableAnimationsComponent
			{
				availableAnimationTypes = availableAnimationTypes
			};
		}
	}
}