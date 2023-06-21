using Ingame.Animation;
using Ingame.Hud;
using Ingame.Interaction.Common;
using Ingame.Movement;
using Leopotam.Ecs;
using Support.Extensions;

namespace Ingame.Inventory
{
	public sealed class PickUpHelmetSystem : IEcsRunSystem
	{
		private readonly EcsFilter<TransformModel, HelmetTag, LootItemTag, PerformInteractionTag> _lootHelmetFilter;
		private readonly EcsFilter<TransformModel, AnimatorModel, HelmetTag>.Exclude<LootItemTag> _helmetInInventoryFilter;

		public void Run()
		{
			if(_lootHelmetFilter.IsEmpty())
				return;
			
			foreach(var i in _lootHelmetFilter)
			{
				ref var transformModel = ref _lootHelmetFilter.Get1(i);
				transformModel.transform.SetGameObjectInactive();
			}

			foreach(var i in _helmetInInventoryFilter)
			{
				_helmetInInventoryFilter.GetEntity(i).Get<InInventoryTag>();
			}
		}
	}
}