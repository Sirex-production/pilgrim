using Leopotam.Ecs;

namespace Ingame.Movement
{
    public class CharacterControllerInitSystem : IEcsInitSystem
    {
        private readonly EcsFilter<CharacterControllerModel> _characterControllerFilter;

        public void Init()
        {
            foreach (var i in _characterControllerFilter)
            {
                ref var characterControllerModel = ref _characterControllerFilter.Get1(i);
                characterControllerModel.initialHeight = characterControllerModel.characterController.height;
            }
        }
    }
}