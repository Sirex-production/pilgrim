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

            _saveDataContainer.player ??= new PlayerData();
            
            _saveDataContainer.player.health = _playerHealthFilter.Get1(0).currentHealth;
            
            var inventory =  _inventoryFilter.Get1(0);
            _saveDataContainer.player.inventory = inventory;
        
            //ammo weapon
            //_saveDataContainer.player.

            var encData = BinarySerializer.SerializeData(_saveDataContainer);
            PlayerPrefs.SetString("save",encData);
            PlayerPrefs.Save();
            _savePlayerFilter.GetEntity(0).Destroy();
        }
    }
}