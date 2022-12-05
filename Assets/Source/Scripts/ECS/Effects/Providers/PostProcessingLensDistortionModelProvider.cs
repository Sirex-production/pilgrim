using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Voody.UniLeo;
using Zenject;

namespace Ingame.Effects
{
    [RequireComponent(typeof(Volume))]
    public sealed class PostProcessingLensDistortionModelProvider : MonoProvider<PostProcessingLensDistortionModel>
    {
        [Inject]
        private void Construct()
        {
            if (!GetComponent<Volume>().profile.TryGet(out LensDistortion attachedLensDistortion))
                throw new NullReferenceException("LensDistortion effect is not present in post processing volume");
            
            value = new PostProcessingLensDistortionModel
            {
                lensDistortion = attachedLensDistortion,
                initialIntensity = attachedLensDistortion.intensity.value,
                initialXMultiplier = attachedLensDistortion.xMultiplier.value,
                initialYMultiplier = attachedLensDistortion.yMultiplier.value,
                initialCenter = attachedLensDistortion.center.value,
                initialScale = attachedLensDistortion.scale.value
            };
        }
    }
}