using Leopotam.Ecs;
using Support;

namespace Ingame
{
    public class EcsWorldDebugListener : IEcsWorldDebugListener
    {
        public void OnEntityCreated(EcsEntity entity)
        {
            // TemplateUtils.SafeDebug("Entity was created");
        }

        public void OnEntityDestroyed(EcsEntity entity)
        {
            // TemplateUtils.SafeDebug("Entity was destroyed");
        }

        public void OnFilterCreated(EcsFilter filter)
        {
            // TemplateUtils.SafeDebug("Filter was created");
        }

        public void OnComponentListChanged(EcsEntity entity)
        {
            // TemplateUtils.SafeDebug("Component list was changed");
        }

        public void OnWorldDestroyed(EcsWorld world)
        {
            // TemplateUtils.SafeDebug("World was destroyed");
        }
    }
}