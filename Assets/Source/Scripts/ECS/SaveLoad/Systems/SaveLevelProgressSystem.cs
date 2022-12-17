using Ingame.Inventory;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ingame.SaveLoad
{
    public sealed class SaveLevelProgressSystem : IEcsRunSystem
    {
        private readonly EcsFilter<SavePlayerProgressEvent> _saveFilter;
        private SaveDataContainer _saveDataContainer;
        public void Run()
        {
            if(_saveFilter.IsEmpty())
                return;
            var index = SceneManager.GetActiveScene().buildIndex;
            _saveDataContainer.level = new LevelData(){level = index};

            var save = BinarySerializer.SerializeData(_saveDataContainer);
            PlayerPrefs.SetString("save", save);
            PlayerPrefs.Save();
            
            _saveFilter.GetEntity(0).Destroy();
        }
    }
}