using Ingame.Movement;
using Leopotam.Ecs;

namespace Ingame.Utils
{
    public class DeltaMovementInitializeSystem : IEcsInitSystem
    {
        private readonly EcsFilter<DeltaMovementComponent> _deltaMovementFilter;

        public void Init()
        {
            foreach (var i in _deltaMovementFilter)
            {
                ref var entity = ref _deltaMovementFilter.GetEntity(i);
                ref var deltaMovementComp = ref _deltaMovementFilter.Get1(i);

                if (entity.Has<TransformModel>())
                    deltaMovementComp.previousPosition = entity.Get<TransformModel>().transform.position;
                else
                    entity.Del<DeltaMovementComponent>();
            }
        }
    }
}