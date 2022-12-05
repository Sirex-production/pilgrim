using Leopotam.Ecs;
using Support;

namespace Ingame.Debuging
{
    public class DebugSystem : IEcsRunSystem
    {
        private readonly EcsFilter<DebugRequest> _debugRequestFilter;
        
        public void Run()
        {
            foreach (var i in _debugRequestFilter)
            {
                ref var debugRequest = ref _debugRequestFilter.Get1(i);
                TemplateUtils.SafeDebug(debugRequest.message);
            }
        }
    }
}