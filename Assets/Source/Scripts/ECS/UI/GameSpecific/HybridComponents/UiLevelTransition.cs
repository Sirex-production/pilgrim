using NaughtyAttributes;
using Support;
using Support.Extensions;
using Support.UI;
using UnityEngine;
using Zenject;

namespace Ingame.UI
{
    public class UiLevelTransition : MonoBehaviour
    {
        [BoxGroup("References"), Required]
        [SerializeField] private Animator levelTransitionAnimator;

        [Inject] private readonly UiController _uiController;
        [Inject] private readonly LevelManager _levelManager;

        private const float ADDITIONAL_PAUSE_BEFORE_TRANSITION = 1.3f;
        
        private void Awake()
        {
            _uiController.OnUiRestartLevelTransition += OnUiRestartLevelTransition;
            _uiController.OnUiLoadNextLevelTransition += OnUiLoadNextLevelTransition;
        }

        private void OnDestroy()
        {
            _uiController.OnUiRestartLevelTransition -= OnUiRestartLevelTransition;
            _uiController.OnUiLoadNextLevelTransition -= OnUiLoadNextLevelTransition;
        }

        private void OnUiRestartLevelTransition()
        {
            levelTransitionAnimator.ResetTrigger("Show");
            levelTransitionAnimator.SetTrigger("Show");

            float pauseBeforeLoadingLevel = levelTransitionAnimator.GetCurrentAnimatorStateInfo(0).length;
            pauseBeforeLoadingLevel += ADDITIONAL_PAUSE_BEFORE_TRANSITION;
            this.WaitAndDoCoroutine(pauseBeforeLoadingLevel, _levelManager.RestartLevel);
        }

        private void OnUiLoadNextLevelTransition()
        {
            levelTransitionAnimator.ResetTrigger("Show");
            levelTransitionAnimator.SetTrigger("Show");
            this.DoAfterNextFrameCoroutine(() => levelTransitionAnimator.ResetTrigger("Show"));
            
            float pauseBeforeLoadingLevel = levelTransitionAnimator.GetCurrentAnimatorStateInfo(0).length;
            pauseBeforeLoadingLevel += ADDITIONAL_PAUSE_BEFORE_TRANSITION;
            this.WaitAndDoCoroutine(pauseBeforeLoadingLevel, _levelManager.LoadNextLevel);
        }
    }
}