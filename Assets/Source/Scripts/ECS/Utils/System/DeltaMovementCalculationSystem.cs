using Ingame.Movement;
using Leopotam.Ecs;

namespace Ingame.Utils
{
    public class DeltaMovementCalculationSystem : IEcsRunSystem
    {
        private readonly EcsFilter<DeltaMovementComponent, TransformModel> _deltaMovementFilter;
        
        public void Run()
        {
            foreach (var i in _deltaMovementFilter)
            {
                ref var deltaMovementComp = ref _deltaMovementFilter.Get1(i);
                ref var transformModel = ref _deltaMovementFilter.Get2(i);

                var currentPos = transformModel.transform.position;

                deltaMovementComp.deltaMovement = deltaMovementComp.previousPosition - currentPos;
                deltaMovementComp.previousPosition = currentPos;
            }
        }
    }
}