using Leopotam.Ecs;

namespace Ingame {
    sealed class InitializeEntityReferenceSystem : IEcsRunSystem
    {
        private EcsFilter<InitializeEntityReferenceRequest> _filter;
        public void Run()
        {
            foreach (var i in _filter)
            {

                ref var entity = ref _filter.GetEntity(i);
                ref var reference = ref _filter.Get1(i);
                reference.entityReference.Entity = entity;
                entity.Del<InitializeEntityReferenceRequest>();
             
            }
        }
    }
}