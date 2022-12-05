using Ingame.Movement;
using Ingame.Player;
using Leopotam.Ecs;

namespace Ingame.Effects
{
    public sealed class PlayerPositionSetterSystem : IEcsRunSystem
    {
        private readonly EcsFilter<PlayerModel, TransformModel, CharacterControllerModel> _playerFilter;
        private readonly EcsFilter<GrassMaterialModel> _grassMaterialModel;

        public void Run()
        {
            if(_playerFilter.IsEmpty())
                return;
            
            var characterControllerQuarterHeight = _playerFilter.Get3(0).characterController.height / 4;
            var playerPosition = _playerFilter.Get2(0).transform.position;

            playerPosition.y -= characterControllerQuarterHeight;
            
            foreach (var i in _grassMaterialModel)
            {
                ref var grassMaterialModel = ref _grassMaterialModel.Get1(i);
                int propertyId = grassMaterialModel.playerWorldPositionShaderPropertyId;
                
                grassMaterialModel.grassMaterial.SetVector(propertyId, playerPosition);
            }
        }
    }
}