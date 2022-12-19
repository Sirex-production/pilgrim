using Ingame.Health;
using Ingame.Inventory;
using Ingame.Movement;
using Ingame.Player;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.SaveLoad
{
    public class LoadProgressSystem : IEcsRunSystem
    {
        private readonly EcsFilter<HealthComponent,PlayerModel,CharacterControllerModel> _playerHealthFilter;
        private readonly EcsFilter<InventoryComponent> _inventoryFilter;
        
        private readonly EcsFilter<LoadProgressEvent> _loadFilter;
        
        private PersistenceDataController _persistenceDataController;
        public void Run()
        {
            if(_loadFilter.IsEmpty())
                return;
            
            
            if (!_persistenceDataController.persistenceDataContainer.LevelPersistence.WasModified || _persistenceDataController.persistenceDataContainer.LevelPersistence.Value == null)
            {
                _loadFilter.GetEntity(0).Destroy();
                return;
            }
            
          
            
            //systems that modifies scene must fetch the value
            
            
            _loadFilter.GetEntity(0).Destroy();
        }

       
    }
}