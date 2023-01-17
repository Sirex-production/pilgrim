using Ingame.Quests;
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
		[BoxGroup("Quest")]
		[Required, SerializeField] private QuestsConfig questsConfig;

		public VfxSurfaceTypeConfig VFXSurfaceTypeConfig => vfxSurfaceTypeConfig;
		
		public CursorConfig CursorConfig => cursorConfig;

		public QuestsConfig QuestsConfig => questsConfig;
	}
}