using Ingame.Inventory;
using Leopotam.Ecs;

namespace Ingame.Hud
{
    public sealed class AppearanceUpdateInitSystem : IEcsInitSystem
    {
        private readonly EcsWorld _world;
        
        public void Init()
        {
            _world.NewEntity().Get<UpdateBackpackAppearanceEvent>();
        }
    }
}