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

            if (!_saveDataContainer.LevelPersistence.WasModified || _saveDataContainer.LevelPersistence.Value == null)
            {
                _loadLevelFilter.GetEntity(0).Destroy();
                return;
            }
            //system that gives a scene-changer an index
            
            _loadLevelFilter.GetEntity(0).Destroy();
        }
    }
}

