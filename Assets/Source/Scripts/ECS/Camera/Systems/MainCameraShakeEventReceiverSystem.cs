using Leopotam.Ecs;

namespace Ingame.CameraWork
{
    public sealed class MainCameraShakeEventReceiverSystem : IEcsRunSystem
    {
        private readonly EcsFilter<VirtualCameraModel, MainVirtualCameraTag> _mainVirtualCamera;
        private readonly EcsFilter<CameraShakeRequest> _cameraShakeRequest;

        public void Run()
        {
            if(_cameraShakeRequest.IsEmpty() || _mainVirtualCamera.IsEmpty())
                return;

            ref var cameraRequestEntity = ref _cameraShakeRequest.GetEntity(0);
            ref var cameraShakeRequest = ref _cameraShakeRequest.Get1(0);
            ref var virtualCameraEntity = ref _mainVirtualCamera.GetEntity(0);

            ref var cameraShakeComp = ref virtualCameraEntity.Get<CameraShakeComponent>();

            cameraShakeComp.duration = cameraShakeRequest.duration;
            cameraShakeComp.amplitude = cameraShakeRequest.amplitude;
            cameraShakeComp.frequency = cameraShakeRequest.frequency;
            
            cameraRequestEntity.Del<CameraShakeRequest>();
        }
    }
}