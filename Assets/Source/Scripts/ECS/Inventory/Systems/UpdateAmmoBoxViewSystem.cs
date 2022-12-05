using Leopotam.Ecs;

namespace Ingame.Inventory
{
	public sealed class UpdateAmmoBoxViewSystem : IEcsRunSystem
	{
		private readonly EcsFilter<AmmoBoxComponent, AmmoBoxViewComponent> _ammoBoxFilter;
		private readonly EcsFilter<UpdateBackpackAppearanceEvent> _updateInventoryEventFilter;
		
		public void Run()
		{
			if(_updateInventoryEventFilter.IsEmpty())
				return;
			
			foreach (var i in _ammoBoxFilter)
			{
				ref var ammoBoxComponent = ref _ammoBoxFilter.Get1(i);
				ref var ammoBoxViewComponent = ref _ammoBoxFilter.Get2(i);

				for (int ammoTypeIndex = 0; ammoTypeIndex < ammoBoxComponent.ammo.Length; ammoTypeIndex++)
				{
					if(ammoBoxViewComponent.ammoAmountTexts[ammoTypeIndex] == null)
						continue;
					
					int amountOfAmmo = ammoBoxComponent.ammo[ammoTypeIndex];
					ammoBoxViewComponent.ammoAmountTexts[ammoTypeIndex].SetText($"{amountOfAmmo}");
				}
			}
		}
	}
}