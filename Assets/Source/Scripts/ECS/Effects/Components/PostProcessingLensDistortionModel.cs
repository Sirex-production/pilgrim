using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Ingame.Effects
{
    public struct PostProcessingLensDistortionModel
    {
        public LensDistortion lensDistortion;

        public float initialIntensity;
        public float initialXMultiplier;
        public float initialYMultiplier;
        public Vector2 initialCenter;
        public float initialScale;
    }
}