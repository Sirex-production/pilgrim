using NaughtyAttributes;
using UnityEngine;
using Voody.UniLeo;
using Zenject;

namespace Ingame.Effects
{
    public sealed class GrassMaterialModelProvider : MonoProvider<GrassMaterialModel>
    {
        [Required, SerializeField] private Material grassMaterial;
        
        [Inject]
        public void Construct()
        {
            value = new GrassMaterialModel
            {
                grassMaterial = grassMaterial,
                playerWorldPositionShaderPropertyId = Shader.PropertyToID("_PlayerWorldPosition")
            };
        }
    }
}