using Ingame.Health;
using Ingame.Inventory;
using Ingame.Movement;
using Ingame.Player;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.SaveLoad
{
    public class LoadPlayerProgressSystem : IEcsRunSystem
    {
        private readonly EcsFilter<HealthComponent,PlayerModel,CharacterControllerModel> _playerHealthFilter;
        private readonly EcsFilter<InventoryComponent> _inventoryFilter;
        
        private readonly EcsFilter<LoadPlayerProgressEvent> _loadPlayerFilter;
        
        private SaveDataContainer _saveDataContainer;
        public void Run()
        {
            if(_loadPlayerFilter.IsEmpty())
                return;
            
            if (!_saveDataContainer.playerPersistence.WasModified || _saveDataContainer.playerPersistence.Value ==null)
            {
                _loadPlayerFilter.GetEntity(0).Destroy();
                return;
            }
            
            ref var health = ref _playerHealthFilter.Get1(0);
            health.currentHealth = _saveDataContainer.playerPersistence.Value.Health;

            if(_saveDataContainer.playerPersistence.Value.Inventory != null){
                ref var inventory = ref _inventoryFilter.Get1(0);
                var newInventory = _saveDataContainer.playerPersistence.Value.Inventory;
                inventory.currentNumberOfAdrenaline = newInventory.CurrentNumberOfAdrenaline;
                inventory.currentNumberOfInhalators = newInventory.CurrentNumberOfBandages;
                inventory.currentNumberOfBandages = newInventory.CurrentNumberOfInhalators;
                inventory.currentNumberOfMorphine = newInventory.CurrentNumberOfMorphine;
                inventory.currentNumberOfCreamTubes = newInventory.CurrentNumberOfCreamTubes;
                inventory.currentNumberOfEnergyDrinks = newInventory.CurrentNumberOfEnergyDrinks;
            }

            _loadPlayerFilter.GetEntity(0).Destroy();
        }

       
    }
}