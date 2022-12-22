using DG.Tweening;
using NaughtyAttributes;
using Support.Extensions;
using UnityEngine;
using Zenject;

namespace Ingame.UI.MainMenu
{
	public sealed class UiWindowSwapper : MonoBehaviour
	{
		[BoxGroup("Refrences")]
		[SerializeField] private RectTransform[] windows;
		[BoxGroup("Animation Properties")]
		[SerializeField] [Min(0)] private float windowChangeAnimationDuration = .3f;
		
		private MainMenuService _mainMenuService;
		private UiWindowType _currentUiWindowType = UiWindowType.None;

		[Inject]
		private void Construct(MainMenuService mainMenuService)
		{
			_mainMenuService = mainMenuService;
		}

		private void Awake()
		{
			_mainMenuService.OnWindowChangeRequested += OnWindowChangeRequested;

			foreach (var windowTransform in windows) 
				windowTransform.SetGameObjectInactive();
			
			HideAllWindows();
		}

		private void OnDestroy()
		{
			_mainMenuService.OnWindowChangeRequested -= OnWindowChangeRequested;
		}

		private void OnWindowChangeRequested(UiWindowType uiWindowType)
		{
			if(_currentUiWindowType == uiWindowType)
				return;

			_currentUiWindowType = uiWindowType;

			if (uiWindowType == UiWindowType.None)
			{
				HideAllWindows();
				return;
			}

			HideAllWindows();
			this.WaitAndDoCoroutine(windowChangeAnimationDuration, () => OpenWindow(uiWindowType));
		}

		private void HideAllWindows()
		{
			foreach (var windowTransform in windows)
			{
				windowTransform.DOKill();
				windowTransform
					.DOScaleY(0, windowChangeAnimationDuration)
					.OnComplete(windowTransform.SetGameObjectInactive);
			}
		}

		private void OpenWindow(UiWindowType uiWindowType)
		{
			int windowIndex = (int)uiWindowType;
			var windowTransform = windows[windowIndex];
			windowTransform.DOKill();
			
			windowTransform.SetGameObjectActive();
			windowTransform.DOScaleY(1, windowChangeAnimationDuration)
				.SetEase(Ease.OutBack);
		}
	}
}