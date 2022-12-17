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
  
            var encData = PlayerPrefs.GetString("save", null);
            if (encData == null)
            {
                _loadPlayerFilter.GetEntity(0).Destroy();
                return;
            }

            var decData = BinarySerializer.DeserializeData(encData);
            if (decData.level != null )
            {
                _saveDataContainer.player ??= new PlayerData();
                _saveDataContainer.player  = decData.player;
              
                ref var health = ref _playerHealthFilter.Get1(0);
                health.currentHealth = _saveDataContainer.player.health;

                ref var inventory = ref _inventoryFilter.Get1(0);
                var newInventory = _saveDataContainer.player.inventory;
                inventory = newInventory;
            }

            _loadPlayerFilter.GetEntity(0).Destroy();
        }

       
    }
}