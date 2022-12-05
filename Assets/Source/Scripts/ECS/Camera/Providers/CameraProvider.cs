using UnityEngine;
using Voody.UniLeo;
using Zenject;

namespace Ingame.CameraWork
{
    [RequireComponent(typeof(Camera))]
    public sealed class CameraProvider : MonoProvider<CameraModel>
    {
        [Inject]
        private void Construct()
        {
            if (gameObject.CompareTag("MainCamera"))
                gameObject.AddComponent<MainCameraTagProvider>();
                
            value = new CameraModel
            {
                camera = GetComponent<Camera>()
            };
        }
    }
}