using Ingame.Health;
using Ingame.Inventory;
using Ingame.Movement;
using Ingame.Player;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ingame.SaveLoad
{
    public class SaveProgressSystem : IEcsRunSystem
    {
        private readonly EcsFilter<AmmoBoxComponent> _ammoFilter;
        private readonly EcsFilter<HealthComponent,PlayerModel,CharacterControllerModel> _playerHealthFilter;
        private readonly EcsFilter<InventoryComponent> _inventoryFilter;
        private readonly EcsFilter<SaveProgressEvent> _savePlayerFilter;
        
        private PersistenceDataController _persistenceDataController;
        public void Run()
        {
            if(_savePlayerFilter.IsEmpty())
                return;
         
            
            //level 
            var index = SceneManager.GetActiveScene().buildIndex;
            _persistenceDataController.persistenceDataContainer.LevelPersistence.Value = new LevelPersistenceData(index);
  
            
            var encData = JsonSerializer.SerializeData(_persistenceDataController.persistenceDataContainer);
            PlayerPrefs.SetString(PersistenceDataController.DATA_PERSISTANCE_NAME,encData);
            PlayerPrefs.Save();
            _savePlayerFilter.GetEntity(0).Destroy();
        }
    }
}