using Ingame.Input;
using Leopotam.Ecs;

namespace Ingame.CameraWork
{
    public sealed class CameraInputToStatesConverterSystem : IEcsRunSystem
    {
        private readonly EcsFilter<MainVirtualCameraTag> _mainCameraFilter;
        private readonly EcsFilter<AimInputEvent> _aimEventFilter;

        public void Run()
        {
            if(_aimEventFilter.IsEmpty())
                return;

            foreach (var i in _mainCameraFilter)
            {
                ref var cameraEntity = ref _mainCameraFilter.GetEntity(i);
                
                if (cameraEntity.Has<CameraIsAimingTag>())
                    cameraEntity.Del<CameraIsAimingTag>();
                else
                    cameraEntity.Get<CameraIsAimingTag>();
            }
        }
    }
}