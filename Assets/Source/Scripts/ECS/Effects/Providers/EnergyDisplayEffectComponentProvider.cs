using UnityEngine;
using UnityEngine.Rendering;
using Voody.UniLeo;
using Zenject;

namespace Ingame.Effects
{
    [RequireComponent(typeof(Volume))]
    public sealed class EnergyDisplayEffectComponentProvider : MonoProvider<EnergyDisplayEffectComponent>
    {
        [SerializeField] [Min(0)] private float effectSpeed = 3f; 
        
        [Inject]
        private void Construct()
        {
            value = new EnergyDisplayEffectComponent
            {
                effectSpeed = this.effectSpeed
            };
        }
    }
}