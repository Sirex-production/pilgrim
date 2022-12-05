using NaughtyAttributes;
using UnityEngine;
using Voody.UniLeo;
using Zenject;

namespace Ingame.Effects
{
    public sealed class Vignette2DMaterialModelProvider : MonoProvider<Vignette2DMaterialModel>
    {
        [Required, SerializeField] private Material vignette2dMaterial;

        [Inject]
        private void Construct()
        {
            value = new Vignette2DMaterialModel
            {
                vignette2DMaterial = vignette2dMaterial,
                RADIUS_PROP_ID = Shader.PropertyToID("_Radius")
            };
        }
    }
}