using UnityEngine;

namespace Ingame.Effects
{
    public struct GasChokeEffectComponent
    {
        public float effectSpeed;
        public float minIntensity;
        public float maxIntensity;
        public float maxCenterOffset;

        public Vector2 targetCenter;
        public float targetIntensity;
    }
}