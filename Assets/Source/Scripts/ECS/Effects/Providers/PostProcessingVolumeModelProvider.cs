using UnityEngine;
using UnityEngine.Rendering;
using Voody.UniLeo;
using Zenject;

namespace Ingame.Effects
{
    [RequireComponent(typeof(Volume))]
    public sealed class PostProcessingVolumeModelProvider : MonoProvider<PostProcessingVolumeModel>
    {
        [Inject]
        private void Construct()
        {
            value = new PostProcessingVolumeModel
            {
                volume = GetComponent<Volume>()
            };
        }
    }
}