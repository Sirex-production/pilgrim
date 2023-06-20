using Ingame.Animation;
using Ingame.Hud;
using Ingame.Input;
using Ingame.Movement;
using Leopotam.Ecs;

namespace Ingame.Inventory
{
	public sealed class PutOnOrOffHelmetSystem : IEcsRunSystem
	{
		private readonly EcsWorld _world;
		private readonly EcsFilter<TransformModel, AnimatorModel, HelmetTag, InInventoryTag> _helmetInInventoryFilter;
		private readonly EcsFilter<HelmetInputEvent> _helmetInputEventFilter;

		public void Run()
		{
			if(_helmetInputEventFilter.IsEmpty())
				return;

			foreach(var i in _helmetInInventoryFilter)
			{
				ref var entity = ref _helmetInInventoryFilter.GetEntity(i);
				ref var animatorModel = ref _helmetInInventoryFilter.Get2(i);
				bool isHelmetOn = entity.Has<HudIsInHandsTag>();

				if(isHelmetOn) 
					entity.Del<HudIsInHandsTag>();
				else
					entity.Get<HudIsInHandsTag>();

				animatorModel.animator.SetBool("IsOn", isHelmetOn);
			}
		}
	}
}