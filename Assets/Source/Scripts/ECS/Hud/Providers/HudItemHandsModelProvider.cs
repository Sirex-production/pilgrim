using NaughtyAttributes;
using UnityEngine;
using Voody.UniLeo;
using Zenject;

namespace Ingame.Hud
{
	public sealed class HudItemHandsModelProvider : MonoProvider<HudItemHandsModel>
	{
		[Required, SerializeField] private Transform handsTransform;
		
		[Inject]
		private void Construct()
		{
			value = new HudItemHandsModel
			{
				handsTransform = handsTransform
			};
		}
	}
}