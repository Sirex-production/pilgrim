using UnityEngine;
using Voody.UniLeo;
using Zenject;

namespace Ingame.UI
{
	public sealed class MagazineAmmoDisplayComponentProvider : MonoProvider<MagazineAmmoDisplayComponent>
	{
		[SerializeField] [Min(0)] private float displayedTime = 1f;
		[SerializeField] [Min(0)] private float showHideLerpingSpeed = 4f;
		[SerializeField] [Min(0)] private int minMaxAmmoDisplayOffset = 3;

		[Inject]
		private void Construct()
		{
			value = new MagazineAmmoDisplayComponent
			{
				displayedTime = displayedTime,
				showHideLerpingSpeed = showHideLerpingSpeed,
				minMaxAmmoDisplayOffset = minMaxAmmoDisplayOffset
			};
		}
	}
}