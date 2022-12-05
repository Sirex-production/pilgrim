using Cinemachine;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.CameraWork
{
    public class CameraShakeSystem : IEcsRunSystem
    {
        private readonly EcsFilter<VirtualCameraModel, CameraShakeComponent> _cameraShakeFilter;
        
        public void Run()
        {
            foreach (var i in _cameraShakeFilter)
            {
                ref var shakingCameraEntity = ref _cameraShakeFilter.GetEntity(i);
                ref var virtualCameraModel = ref _cameraShakeFilter.Get1(i);
                ref var cameraShakeComp = ref _cameraShakeFilter.Get2(i);
                var vCamNoise = virtualCameraModel.virtualCameraNoise;
                
                if (vCamNoise == null)
                {
                    shakingCameraEntity.Del<CameraShakeComponent>();
                    continue;
                }
                
                cameraShakeComp.timePassedShaking += Time.deltaTime;
                if (cameraShakeComp.timePassedShaking > cameraShakeComp.duration)
                {
                    vCamNoise.m_AmplitudeGain = 0;
                    vCamNoise.m_AmplitudeGain = 0;
                    
                    shakingCameraEntity.Del<CameraShakeComponent>();
                    
                    continue;
                }
                
                vCamNoise.m_AmplitudeGain = cameraShakeComp.amplitude;
                vCamNoise.m_FrequencyGain = cameraShakeComp.frequency;
            }
        }
    }
}