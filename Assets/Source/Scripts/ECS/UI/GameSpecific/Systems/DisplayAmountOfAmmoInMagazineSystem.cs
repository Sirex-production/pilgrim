using Ingame.Gunplay;
using Ingame.Hud;
using Ingame.Input;
using Ingame.Utils;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.UI
{
	public sealed class DisplayAmountOfAmmoInMagazineSystem : IEcsRunSystem
	{
		private readonly EcsFilter<TmpTextModel, CanvasGroupModel, TimerComponent, MagazineAmmoDisplayComponent> _displayTextFilter;
		private readonly EcsFilter<MagazineComponent, InInventoryTag, HudIsInHandsTag> _inHandsMagazineFilter;
		
		private readonly EcsFilter<ShowAmountOfAmmoInputEvent> _showAmmoInputEventFilter;

		public void Run()
		{
			if(_inHandsMagazineFilter.IsEmpty())
				return;

			ref var magazineComponent = ref _inHandsMagazineFilter.Get1(0);

			foreach (var i in _displayTextFilter)
			{
				ref var displayText = ref _displayTextFilter.Get1(i);
				ref var timerComponent = ref _displayTextFilter.Get3(i);
				ref var magazineAmmoDisplayComponent = ref _displayTextFilter.Get4(i);
				var canvasGroup = _displayTextFilter.Get2(i).canvasGroup;
				string textToDisplay = null;

				canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha,
					timerComponent.timePassed > magazineAmmoDisplayComponent.displayedTime ? 0 : 1,
					magazineAmmoDisplayComponent.showHideLerpingSpeed * Time.deltaTime);

				if (_showAmmoInputEventFilter.IsEmpty())
					return;

				timerComponent.timePassed = 0f;

				if (magazineComponent.currentAmountOfAmmo < 3)
					textToDisplay = "~0";
				else if (magazineComponent.currentAmountOfAmmo > magazineComponent.maximumAmmoCapacity - 3)
					textToDisplay = "~full";
				else if (magazineComponent.currentAmountOfAmmo > magazineComponent.maximumAmmoCapacity / 2)
					textToDisplay = ">1/2";
				else
					textToDisplay = "<1/2";

				displayText.tmpText.SetText(textToDisplay);
			}
		}
	}
}