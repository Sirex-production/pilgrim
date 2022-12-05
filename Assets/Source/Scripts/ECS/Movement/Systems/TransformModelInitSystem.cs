using Leopotam.Ecs;

namespace Ingame.Movement
{
    public sealed class TransformModelInitSystem : IEcsInitSystem
    {
        private readonly EcsFilter<TransformModel> _transformFilter;

        public void Init()
        {
            foreach (var i in _transformFilter)
            {
                ref var transformModel = ref _transformFilter.Get1(i);
                var transform = transformModel.transform;

                transformModel.initialLocalPos = transform.localPosition;
                transformModel.initialLocalRotation = transform.localRotation;
            }
        }
    }
}