using Cinemachine;
using UnityEngine;
using Voody.UniLeo;
using Zenject;

namespace Ingame.CameraWork
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class VirtualCameraModelProvider : MonoProvider<VirtualCameraModel>
    {
        [Inject]
        private void Construct()
        {
            var vCam = GetComponent<CinemachineVirtualCamera>();
            var vCamNoise = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            
            value = new VirtualCameraModel
            {
                virtualCamera = vCam,
                virtualCameraNoise = vCamNoise
            };
        }
    }
}