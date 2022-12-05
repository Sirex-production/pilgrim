using System;

namespace Ingame.Animation
{
	public struct AvailableAnimationsComponent
	{
		public AnimationType availableAnimationTypes;

		public bool HasAnimation(AnimationType animationType) => (availableAnimationTypes & animationType) != 0;
	}

	[Serializable, Flags]
	public enum AnimationType
	{
		None = 0,
		Aim = 1 << 0,
		Reload = 1 << 1,
		DistortTheShutter = 1 << 2,
		ShutterDelay = 1 << 3,
		MagazineCheck = 1 << 4,
		ShutterDelayLayer = 1 << 5
	}
}