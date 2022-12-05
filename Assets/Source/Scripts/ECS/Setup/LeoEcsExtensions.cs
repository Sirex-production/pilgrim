using Ingame.Debuging;
using Leopotam.Ecs;

namespace Ingame
{
    public static class LeoEcsExtensions
    {
        public static void SendDebugMessage(this EcsWorld ecsWorld, string message)
        {
            ecsWorld.NewEntity().Get<DebugRequest>().message = message;
        }
    }
}