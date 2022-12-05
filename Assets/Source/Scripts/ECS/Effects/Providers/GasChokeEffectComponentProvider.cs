using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Voody.UniLeo;
using Zenject;

namespace Ingame.Effects
{
    [RequireComponent(typeof(Volume))]
    public sealed class GasChokeEffectComponentProvider : MonoProvider<GasChokeEffectComponent>
    {
        [SerializeField] [Min(0)] private float effectSpeed;
        [SerializeField] [MinMaxSlider(0, 1)] private Vector2 minMaxIntensity; 
        [SerializeField] [Range(0, 5)] private float maxCenterOffset;
        
        [Inject]
        private void Construct()
        {
            if (!GetComponent<Volume>().profile.TryGet(out LensDistortion postProcessingVignette))
                throw new NullReferenceException("LensDistortion effect is not present in post processing volume");
            
            value = new GasChokeEffectComponent
            {
                effectSpeed = this.effectSpeed,
                minIntensity = minMaxIntensity.x,
                maxIntensity = minMaxIntensity.y,
                maxCenterOffset = this.maxCenterOffset
            };
        }
    }
}