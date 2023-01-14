using UnityEngine;

namespace Ingame.Inventory
{
	/// <summary>
	/// Component is responsible for overriding layer for game objects when player picks up item.
	/// By default is used with some particles that can not be rendered in HUD layer properly (like distortion) 
	/// </summary>
	public struct OverrideLayerOnPickupAndDropComponent
	{
		public GameObject[] gameObjects;
		public int layerToAssignOnPickup;
		public int layerToAssignOnDrop;
	}
}