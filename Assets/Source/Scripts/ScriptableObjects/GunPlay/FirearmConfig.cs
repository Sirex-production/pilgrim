using NaughtyAttributes;
using UnityEngine;

namespace Ingame.Gunplay
{
	[CreateAssetMenu(menuName = "GunPlay/FirearmConfig", fileName = "NewFirearmConfig")]
	public sealed class FirearmConfig : ScriptableObject
	{
		[BoxGroup("Damage")]
		[SerializeField] private float damage;
		[BoxGroup("Recoil")]
		[SerializeField] private Vector2 recoilBoost;
		[BoxGroup("Recoil")]
		[SerializeField] [Range(0f, 10)] private float recoilStabilizationSpeed;
		
		public float Damage => damage;
		public Vector2 RecoilBoost => recoilBoost;
		public float RecoilStabilizationSpeed => recoilStabilizationSpeed;
	}
}