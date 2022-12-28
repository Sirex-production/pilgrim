using DG.Tweening;
using NaughtyAttributes;
using Support;
using Support.Extensions;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Ingame.UI.Loading
{
    public sealed class UiLoading : MonoBehaviour
    {
        [BoxGroup("References")] 
        [Required, SerializeField] private CanvasGroup screenParentCanvasGroup;
        [BoxGroup("References")]
        [Required, SerializeField] private Image bulletImage;

        [BoxGroup("AnimationProperties")]
        [SerializeField] [Min(0)] private float fadeAnimationDuration = .5f;
        [BoxGroup("AnimationProperties")]
        [SerializeField] [Min(0)] private float loadingAnimationDuration = .3f;
        
        private LevelManagementService _levelManagementService;

        private Sequence _loadingAnimationSequence;

        [Inject]
        private void Construct(LevelManagementService levelManagementService)
        {
            _levelManagementService = levelManagementService;
        }

        private void Awake()
        {
            _levelManagementService.OnLoadingStarted += OnLoadingStarted;
            _levelManagementService.OnLoadingFinished += OnLoadingFinished;
            
            screenParentCanvasGroup.SetGameObjectInactive();
            screenParentCanvasGroup.alpha = 0f;
        }

        private void OnDestroy()
        {
            _levelManagementService.OnLoadingStarted -= OnLoadingStarted;
            _levelManagementService.OnLoadingFinished -= OnLoadingFinished;
        }

        private void OnLoadingStarted()
        {
            ShowLoadingScreen();
        }
        
        private void OnLoadingFinished()
        {
            HideLoadingScreen();
        }

        private void ShowLoadingScreen()
        {
            screenParentCanvasGroup.SetGameObjectActive();
            screenParentCanvasGroup.DOFade(1, fadeAnimationDuration)
                .OnComplete(ShowLoadingAnimation);
        }

        private void HideLoadingScreen()
        {
            if(_loadingAnimationSequence != null)
                _loadingAnimationSequence.Kill();
            
            screenParentCanvasGroup.DOFade(0, fadeAnimationDuration)
                .OnComplete(screenParentCanvasGroup.SetGameObjectInactive);
        }
        
        private void ShowLoadingAnimation()
        {
            if(_loadingAnimationSequence != null)
                _loadingAnimationSequence.Kill();
            
            _loadingAnimationSequence = DOTween.Sequence()
                .Append(bulletImage.transform.DOLocalRotate(Vector3.forward * 360, loadingAnimationDuration, RotateMode.LocalAxisAdd))
                .SetEase(Ease.OutBack)
                .SetLoops(-1);
        }
    }
}