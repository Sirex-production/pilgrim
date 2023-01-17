using Ingame.Settings;
using Ingame.VFX;
using NaughtyAttributes;
using UnityEngine;

namespace Ingame.ConfigProvision
{
	public sealed class ConfigProviderService : MonoBehaviour
	{
		[BoxGroup("VFX")]
		[Required, SerializeField] private VfxSurfaceTypeConfig vfxSurfaceTypeConfig;
		[BoxGroup("Settings")]
		[Required, SerializeField] private CursorConfig cursorConfig;

		public VfxSurfaceTypeConfig VFXSurfaceTypeConfig => vfxSurfaceTypeConfig;
		
		public CursorConfig CursorConfig => cursorConfig;
	}
}