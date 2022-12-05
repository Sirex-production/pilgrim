using DG.Tweening;
using NaughtyAttributes;
using Support.Extensions;
using UnityEngine;

namespace Ingame.UI
{
    public sealed class UiMainMenuButtonBehaviour : MonoBehaviour
    {
        [BoxGroup("Animation settings")]
        [SerializeField] [Min(0)] private float animationDuration = .2f; 
        [BoxGroup("References"), Required]
        [SerializeField] private CanvasGroup mainScreenCanvasGroup;
        [BoxGroup("References"), Required]
        [SerializeField] private CanvasGroup settingsCanvasGroup;

        private const float ADDITIONA_TIME_OFFSET_BEFORE_TURNING_OFF_GAME_OBJECT = .3f;
        
        private Vector3 _initialMainScreenLocalScale; 
        private Vector3 _initialSettingsScreenLocalScale; 
        
        private void Awake()
        {
            _initialMainScreenLocalScale = mainScreenCanvasGroup.transform.localScale;
            _initialSettingsScreenLocalScale = settingsCanvasGroup.transform.localScale;
        }

        public void ShowSettings()
        {
            HideMenu(mainScreenCanvasGroup);
            this.WaitAndDoCoroutine(animationDuration, () => ShowMenu(settingsCanvasGroup, _initialSettingsScreenLocalScale));
        }

        public void ShowMainScreen()
        {
            HideMenu(settingsCanvasGroup);
            this.WaitAndDoCoroutine(animationDuration, () => ShowMenu(mainScreenCanvasGroup, _initialMainScreenLocalScale));
        }

        private void ShowMenu(CanvasGroup canvasGroup, Vector3 initialScale)
        {
            canvasGroup.SetGameObjectActive();
            canvasGroup.alpha = 0;
            canvasGroup.transform.localScale = Vector3.zero;

            canvasGroup.DOFade(1, animationDuration / 2);
            canvasGroup.transform.DOScale(initialScale, animationDuration);
        }

        private void HideMenu(CanvasGroup canvasGroup)
        {
            canvasGroup.SetGameObjectActive();

            canvasGroup.DOFade(0, animationDuration / 2);
            canvasGroup.transform
                .DOScale(Vector3.zero, animationDuration)
                .OnComplete(() => this.WaitAndDoCoroutine(ADDITIONA_TIME_OFFSET_BEFORE_TURNING_OFF_GAME_OBJECT, canvasGroup.SetGameObjectInactive));
        }
    }
}