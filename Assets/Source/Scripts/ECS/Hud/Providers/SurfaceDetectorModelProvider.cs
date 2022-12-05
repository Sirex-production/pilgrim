using Ingame.Utils;
using NaughtyAttributes;
using UnityEngine;
using Voody.UniLeo;
using Zenject;

namespace Ingame.Hud
{
    public sealed class SurfaceDetectorModelProvider : MonoProvider<SurfaceDetectorModel>
    {
        [Required, SerializeField] private SurfaceDetector surfaceDetector;

        [Inject]
        private void Construct()
        {
            value = new SurfaceDetectorModel
            {
                surfaceDetector = surfaceDetector
            };
        }
    }
}