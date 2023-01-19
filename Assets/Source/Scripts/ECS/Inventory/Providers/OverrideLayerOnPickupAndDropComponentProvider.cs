using NaughtyAttributes;
using UnityEngine;
using Voody.UniLeo;
using Zenject;

namespace Ingame.Inventory
{
	public sealed class OverrideLayerOnPickupAndDropComponentProvider : MonoProvider<OverrideLayerOnPickupAndDropComponent>
	{
		[SerializeField] private GameObject[] gameObjects;
		[SerializeField, Layer] private int layerToAssignOnPickup;
		[SerializeField, Layer] private int layerToAssignOnDrop;
		
		[Inject]
		private void Construct()
		{
			value = new OverrideLayerOnPickupAndDropComponent
			{
				gameObjects = gameObjects,
				layerToAssignOnDrop = layerToAssignOnDrop,
				layerToAssignOnPickup = layerToAssignOnPickup
			};
		}
	}
}