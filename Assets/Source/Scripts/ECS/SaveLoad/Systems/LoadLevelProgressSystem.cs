using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Ingame.SaveLoad;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.SaveLoad
{
    public sealed class LoadLevelProgressSystem : IEcsRunSystem
    {
        private readonly EcsFilter<LoadLevelProgressEvent> _loadLevelFilter;
        private SaveDataContainer _saveDataContainer;
        public void Run()
        {
            if(_loadLevelFilter.IsEmpty())
            return;
            var encData = PlayerPrefs.GetString("save", null);
            if (encData == null)
            {
                _loadLevelFilter.GetEntity(0).Destroy();
                return;
            }

            var decData= BinarySerializer.DeserializeData(encData);
            if (decData.level !=null )
            {
                _saveDataContainer.level ??= new LevelData();
                _saveDataContainer.level.level  = decData.level.level;
            }
            
            _loadLevelFilter.GetEntity(0).Destroy();
        }
    }
}

