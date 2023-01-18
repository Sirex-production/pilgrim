#if UNITY_EDITOR
using Leopotam.Ecs;

namespace Ingame
{
    public class EcsWorldDebugListener : IEcsWorldDebugListener
    {
        public void OnEntityCreated(EcsEntity entity)
        {
            // TemplateUtils.SafeDebug("entity was created");
        }

        public void OnEntityDestroyed(EcsEntity entity)
        {
            // TemplateUtils.SafeDebug("entity was destroyed");
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
            // TemplateUtils.SafeDebug("world was destroyed");
        }
    }
}
#endif