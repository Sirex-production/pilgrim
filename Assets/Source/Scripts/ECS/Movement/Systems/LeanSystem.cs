using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Movement
{
    public sealed class LeanSystem : IEcsRunSystem
    {
        private readonly EcsFilter<TransformModel, LeanCallback> _leanFilter;

        public void Run()
        {
            foreach (var i in _leanFilter)
            {
                ref var leanEntity = ref _leanFilter.GetEntity(i);
                ref var transformModel = ref _leanFilter.Get1(i);
                ref var leanReq = ref _leanFilter.Get2(i); ;

                var localRotation = transformModel.transform.localRotation;
                var targetRotation = transformModel.initialLocalRotation *
                                     Quaternion.AngleAxis(leanReq.angle, leanReq.rotationAxis);

                if (Quaternion.Angle(localRotation, targetRotation) < .01f)
                {
                    leanEntity.Del<LeanCallback>();
                    return;   
                }

                transformModel.transform.localRotation = Quaternion.Lerp(localRotation, targetRotation, leanReq.speed * Time.fixedDeltaTime);
            }
        }
    }
}