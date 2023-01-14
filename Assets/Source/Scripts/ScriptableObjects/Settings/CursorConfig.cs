using NaughtyAttributes;
using Support;
using UnityEngine;

namespace Ingame.Settings
{
	[CreateAssetMenu(menuName = "Ingame/Settings/CursorConfig")]
	public sealed class CursorConfig : ScriptableObject
	{
		[InfoBox("Index of array identifies scene index. And value identifies whether cursor is visible when scene is launched")]
		[SerializeField] private bool[] cursorVisibility;

		public bool IsCursorVisibleOnSceneLaunch(int sceneIndex)
		{
			if (sceneIndex >= cursorVisibility.Length || sceneIndex < 0)
			{
				TemplateUtils.SafeDebug($"Scene index configuration does not exists for index {sceneIndex} in {nameof(CursorConfig)}");
				return false;
			}

			return cursorVisibility[sceneIndex];
		}
	}
}