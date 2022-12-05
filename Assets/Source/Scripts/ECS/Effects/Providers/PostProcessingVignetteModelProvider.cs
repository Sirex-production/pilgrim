using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Voody.UniLeo;
using Zenject;

namespace Ingame.Effects
{
    [RequireComponent(typeof(Volume))]
    public sealed class PostProcessingVignetteModelProvider : MonoProvider<PostProcessingVignetteModel>
    {
        [Inject]
        private void Construct()
        {
            if (!GetComponent<Volume>().profile.TryGet(out Vignette postProcessingVignette))
                throw new NullReferenceException("Vignette effect is not present in post processing volume");
            
            value = new PostProcessingVignetteModel
            {
                vignette = postProcessingVignette,
                initialIntensity = postProcessingVignette.intensity.value
            };
        }
    }
}