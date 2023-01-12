using Leopotam.Ecs;

namespace Ingame.Extensions
{
	public static class EcsWorldExtensions
	{
		public static void SendSignal<T>(this EcsWorld ecsWorld) where T : struct
		{
			ecsWorld.NewEntity().Get<T>();
		}
		
		public static void SendSignal<T>(this EcsWorld ecsWorld, in T component) where T : struct
		{
			ref var comp = ref ecsWorld.NewEntity().Get<T>();
			comp = component;
		}
	}
}