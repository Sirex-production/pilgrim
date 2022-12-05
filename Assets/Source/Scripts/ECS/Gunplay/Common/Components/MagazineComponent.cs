namespace Ingame.Gunplay
{
	public struct MagazineComponent
	{
		public AmmoType ammoType;
		public int currentAmountOfAmmo;
		public int maximumAmmoCapacity;
	}

	public enum AmmoType
	{
		A5_56 = 0,
		A7_62 = 1,
		A9_19 = 2
	}
}