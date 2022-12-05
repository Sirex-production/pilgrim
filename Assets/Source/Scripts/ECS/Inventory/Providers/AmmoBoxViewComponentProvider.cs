using System;
using Ingame.Gunplay;
using Support;
using TMPro;
using UnityEngine;
using Voody.UniLeo;
using Zenject;

namespace Ingame.Inventory
{
	public sealed class AmmoBoxViewComponentProvider : MonoProvider<AmmoBoxViewComponent>
	{
		[SerializeField] private TMP_Text[] ammoAmountTexts;

		[Inject]
		private void Construct()
		{
			int amountOfAmmoTypes = Enum.GetValues(typeof(AmmoType)).Length;
			
			if (ammoAmountTexts.Length != amountOfAmmoTypes)
			{
				TemplateUtils.SafeDebug($"Amount of ammo types texts does not correspond to initial amount of ammo types in {nameof(AmmoBoxViewComponentProvider)}", LogType.Error);
				
				Array.Resize(ref ammoAmountTexts, amountOfAmmoTypes);
			}
			
			value = new AmmoBoxViewComponent
			{
				ammoAmountTexts = ammoAmountTexts
			};
		}
	}
}