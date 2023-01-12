using Ingame.VFX;
using NaughtyAttributes;
using UnityEngine;

namespace Ingame.ConfigProvision
{
	public sealed class ConfigProviderService : MonoBehaviour
	{
		[Required, SerializeField] private VfxSurfaceTypeConfig vfxSurfaceTypeConfig;

		public VfxSurfaceTypeConfig VFXSurfaceTypeConfig => vfxSurfaceTypeConfig;
	}
}