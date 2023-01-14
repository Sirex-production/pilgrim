using System;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Ingame.VFX
{
	[CreateAssetMenu(menuName = "Ingame/VFX/VfxSurfaceTypeConfig")]
	public sealed class VfxSurfaceTypeConfig : ScriptableObject
	{
		[InfoBox("Default value will be used if tag will not be found in the dictionary")]
		[SerializeField] private SurfaceVfxData defaultSurfaceVfxData;
		[InfoBox("Use tag name to identify the key for the surface data")]
		[SerializeField] private StringSurfaceVfxDictionary surfaceVfxDictionary;

		public SurfaceVfxData GetVfxSurfaceDataDueToSurfaceTag(in string tag)
		{
			if (surfaceVfxDictionary.ContainsKey(tag))
				return surfaceVfxDictionary[tag];

			return defaultSurfaceVfxData;
		}
	}

	[Serializable]
	public struct SurfaceVfxData
	{
		public Material[] bulletHoleMaterials;
		public ParticleSystem[] shotParticleSystemPrefabs;

		public Material RandomBulletHoleMaterial => bulletHoleMaterials[Random.Range(0, bulletHoleMaterials.Length)];
		public ParticleSystem RandomShotParticleSystemPrefabs => shotParticleSystemPrefabs[Random.Range(0, shotParticleSystemPrefabs.Length)];
	}

	[Serializable]
	public sealed class StringSurfaceVfxDictionary : SerializableDictionary<string, SurfaceVfxData> { }
}