namespace Ingame.Inventory
{
	public struct AmmoBoxComponent
	{
		/// <summary>
		/// Index of that array corresponds to AmmoType enum type index. For example: 0 - 5.55, 1 - 7.62 and so on
		/// </summary>
		public int[] ammo;
	}
}