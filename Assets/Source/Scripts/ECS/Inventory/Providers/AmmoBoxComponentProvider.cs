using System;
using Ingame.Gunplay;
using Support;
using UnityEngine;
using Voody.UniLeo;
using Zenject;

namespace Ingame.Inventory
{
	public sealed class AmmoBoxComponentProvider : MonoProvider<AmmoBoxComponent>
	{
		[SerializeField] private int[] ammo;
		
		[Inject]
		private void Construct()
		{
			int amountOfAmmoTypes = Enum.GetValues(typeof(AmmoType)).Length;

			if (ammo.Length != amountOfAmmoTypes)
			{
				TemplateUtils.SafeDebug($"Amount of ammo types does not correspond to initial amount of ammo types in {nameof(AmmoBoxComponentProvider)}", LogType.Error);
				
				Array.Resize(ref ammo, amountOfAmmoTypes);
			}

			value = new AmmoBoxComponent
			{
				ammo = ammo
			};
		}
	}
}