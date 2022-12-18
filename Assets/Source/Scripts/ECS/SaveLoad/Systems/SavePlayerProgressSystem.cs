using Ingame.Health;
using Ingame.Inventory;
using Ingame.Movement;
using Ingame.Player;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.SaveLoad
{
    public class SavePlayerProgressSystem : IEcsRunSystem
    {
        private readonly EcsFilter<AmmoBoxComponent> _ammoFilter;
        private readonly EcsFilter<HealthComponent,PlayerModel,CharacterControllerModel> _playerHealthFilter;
        private readonly EcsFilter<InventoryComponent> _inventoryFilter;
        private readonly EcsFilter<SavePlayerProgressEvent> _savePlayerFilter;
        
        private SaveDataContainer _saveDataContainer;
        public void Run()
        {
            if(_savePlayerFilter.IsEmpty())
                return;
         
            _saveDataContainer.playerPersistence.Value.Health = _playerHealthFilter.Get1(0).currentHealth;
            
            var inventory =  _inventoryFilter.Get1(0);
            _saveDataContainer.playerPersistence.Value.Inventory = new InventoryPersistenceData(inventory.currentNumberOfAdrenaline, 
                inventory.currentNumberOfBandages, 
                inventory.currentNumberOfInhalators, 
                inventory.currentNumberOfMorphine, 
                inventory.currentNumberOfCreamTubes, 
                inventory.currentNumberOfEnergyDrinks
                );
            
        
            //ammo weapon
            

            var encData = BinarySerializer.SerializeData(_saveDataContainer.playerPersistence.Value);
            PlayerPrefs.SetString(SaveDataContainer.PLAYER_PERSISTANCE_NAME,encData);
            PlayerPrefs.Save();
            _savePlayerFilter.GetEntity(0).Destroy();
        }
    }
}