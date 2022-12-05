using UnityEngine;
using UnityEngine.UI;
using Voody.UniLeo;
using Zenject;

namespace Ingame.UI
{
    [RequireComponent(typeof(Image))]
    public sealed class ImageModelProvider : MonoProvider<ImageModel>
    {
        [Inject]
        private void Construct()
        {
            value = new ImageModel
            {
                image = GetComponent<Image>()
            };
        }
    }
}