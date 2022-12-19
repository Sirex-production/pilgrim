using Leopotam.Ecs;

namespace Ingame.SaveLoad
{
    public class SaveLoadController
    {
        private EcsWorld _world;

        public SaveLoadController(EcsWorld world)
        {
            _world = world;
        }

        public void Load()
        {
            _world.NewEntity().Get<LoadProgressEvent>();
        }

        public void Clear()
        {
            _world.NewEntity().Get<ClearProgressEvent>();
        }
        
        public void Save()
        {
            _world.NewEntity().Get<SaveProgressEvent>();
        }
    }
}