using UnityEngine;
using Voody.UniLeo;
using Zenject;

namespace Ingame.Gunplay
{
	public sealed class MagazineComponentProvider : MonoProvider<MagazineComponent>
	{
		[SerializeField] private AmmoType ammoType;
		[SerializeField] [Min(0)] private int initialAmountOfAmmo;
		[SerializeField] [Min(0)] private int maximumAmmoCapacity;

		[Inject]
		private void Construct()
		{
			value = new MagazineComponent
			{
				ammoType = ammoType,
				currentAmountOfAmmo = initialAmountOfAmmo,
				maximumAmmoCapacity = maximumAmmoCapacity
			};
		}
	}
}