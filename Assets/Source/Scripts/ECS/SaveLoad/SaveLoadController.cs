using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.SaveLoad
{
    public class SaveLoadController
    {
        private EcsWorld _world;

        public SaveLoadController(EcsWorld world)
        {
            _world = world;
        }

        public void LoadLevel()
        {
            _world.NewEntity().Get<LoadLevelProgressEvent>();
        }
        public void LoadPlayer()
        {
            _world.NewEntity().Get<LoadPlayerProgressEvent>();
        }

        public void SaveLevel()
        {
            _world.NewEntity().Get<SaveLevelProgressEvent>();
        }
        public void SavePlayer()
        {
            _world.NewEntity().Get<SavePlayerProgressEvent>();
        }
    }
}