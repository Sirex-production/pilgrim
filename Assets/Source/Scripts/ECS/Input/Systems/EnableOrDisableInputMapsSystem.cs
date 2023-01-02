using System.Runtime.CompilerServices;
using Leopotam.Ecs;
using Support;

namespace Ingame.Input
{
	public sealed class EnableOrDisableInputMapsSystem : IEcsRunSystem
	{
		private readonly StationaryInput _stationaryInput;

		private readonly EcsFilter<DisableFpsInputEvent> _disableFpsInputFilter;
		private readonly EcsFilter<EnableFpsInputEvent> _enableFpsInputFilter;

		public void Run()
		{
			ManageFpsInputMap();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void ManageFpsInputMap()
		{
			bool shouldFpsBeDisabled = !_disableFpsInputFilter.IsEmpty();  
			bool shouldFpsBeEnabled = !_enableFpsInputFilter.IsEmpty();  
			
			if(!shouldFpsBeDisabled && !shouldFpsBeEnabled)
				return;

			if (shouldFpsBeDisabled)
			{
				_stationaryInput.FPS.Disable();
				
				foreach (var i in _disableFpsInputFilter)
				{
					ref var eventEntity = ref _disableFpsInputFilter.GetEntity(i);
					eventEntity.Del<DisableFpsInputEvent>();
				}
			}
			
			if (shouldFpsBeEnabled)
			{
				_stationaryInput.FPS.Enable();
				
				foreach (var i in _enableFpsInputFilter)
				{
					ref var eventEntity = ref _enableFpsInputFilter.GetEntity(i);
					eventEntity.Del<EnableFpsInputEvent>();
				}
			}
		}
	}
}