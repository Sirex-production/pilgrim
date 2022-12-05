using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Voody.UniLeo;
using Zenject;

namespace Ingame.Effects
{
    [RequireComponent(typeof(Volume))]
    public class PostProcessingChromaticAberrationModelProvider : MonoProvider<PostProcessingChromaticAberrationModel>
    {
        [Inject]
        private void Construct()
        {
            if (!GetComponent<Volume>().profile.TryGet(out ChromaticAberration attachedChromaticAberration))
                throw new NullReferenceException("ChromaticAberration effect is not present in post processing volume");
            
            value = new PostProcessingChromaticAberrationModel
            {
                chromaticAberration = attachedChromaticAberration,
                initialIntensity = attachedChromaticAberration.intensity.value
            };
        }
    }
}