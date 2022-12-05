using System;

namespace Ingame.Utils
{
    [Serializable]
    public struct UpdateSettingsRequest
    {
        public bool isCursorAvailable;
        public bool isAimDotVisible;
    }
}