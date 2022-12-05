using NaughtyAttributes;
using UnityEngine;
using Voody.UniLeo;
using Zenject;

namespace Ingame.Gunplay
{
    public sealed class RifleComponentProvider : MonoProvider<RifleComponent>
    {
        [Expandable]
        [Required, SerializeField] private RifleConfig rifleConfig;
        
        [Inject]
        private void Construct()
        {
            value = new RifleComponent
            {
                rifleConfig = rifleConfig
            };
        }
    }
}