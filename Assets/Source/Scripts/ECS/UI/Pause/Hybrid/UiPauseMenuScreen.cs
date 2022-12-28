using DG.Tweening;
using Ingame.UI.Settings;
using NaughtyAttributes;
using Support.Extensions;
using UnityEngine;
using Zenject;

namespace Ingame.UI.Pause
{
	public sealed class UiPauseMenuScreen : MonoBehaviour
	{
		[BoxGroup("References")]
		[Required, SerializeField] private CanvasGroup pauseCanvasGroup;

		[BoxGroup("Animation properties")] 
		[SerializeField] [Min(0)] private float showHideAnimationDuration = .5f;

		private PauseMenuService _pauseMenuService;
		private UiSettingsService _uiSettingsService;
		
		[Inject]
		private void Construct(PauseMenuService pauseMenuService, UiSettingsService uiSettingsService)
		{
			_pauseMenuService = pauseMenuService;
			_uiSettingsService = uiSettingsService;
		}

		private void Awake()
		{
			_pauseMenuService.OnPauseMenuShowRequested += OnPauseMenuShowRequested;
			_pauseMenuService.OnPauseMenuHideRequested += OnPauseMenuHideRequested;

			pauseCanvasGroup.alpha = 0f;
			pauseCanvasGroup.transform.localScale = Vector3.zero;
			pauseCanvasGroup.SetGameObjectInactive();
		}

		private void OnDestroy()
		{
			_pauseMenuService.OnPauseMenuShowRequested -= OnPauseMenuShowRequested;
			_pauseMenuService.OnPauseMenuHideRequested -= OnPauseMenuHideRequested;
		}

		private void OnPauseMenuShowRequested()
		{
			pauseCanvasGroup.SetGameObjectActive();

			pauseCanvasGroup.DOKill();
			pauseCanvasGroup.transform.DOKill();
			
			pauseCanvasGroup.DOFade(1, showHideAnimationDuration);
			pauseCanvasGroup.transform.DOScale(Vector3.one, showHideAnimationDuration)
				.SetEase(Ease.OutBack);
		}

		private void OnPauseMenuHideRequested()
		{
			_uiSettingsService.RequestCloseSettingsWindow();
			
			pauseCanvasGroup.DOKill();
			pauseCanvasGroup.transform.DOKill();
			
			pauseCanvasGroup.DOFade(0, showHideAnimationDuration);
			pauseCanvasGroup.transform.DOScale(Vector3.zero, showHideAnimationDuration)
				.OnComplete(()=> pauseCanvasGroup.SetGameObjectInactive());
		}
	}
}