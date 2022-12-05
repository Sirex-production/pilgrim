using DG.Tweening;
using NaughtyAttributes;
using Support;
using Support.Extensions;
using UnityEngine;
using Zenject;

namespace Ingame.UI
{
    public sealed class UiAfterLevel : MonoBehaviour
    {
        [BoxGroup("References"), Required]
        [SerializeField] private CanvasGroup winScreenCanvasGroup;
        [BoxGroup("References"), Required]
        [SerializeField] private CanvasGroup looseScreenCanvasGroup;
        [BoxGroup("References"), Required]
        [SerializeField] private Transform levelTransitionTransform;
        [BoxGroup("Animation properties")]
        [SerializeField] [Min(0)] private float animationDuration = .3f; 
        
        [Inject] private readonly GameController _gameController;

        private void Awake()
        {
            _gameController.OnLevelEnded += OnLevelEnded;
            
            levelTransitionTransform.SetGameObjectActive();
            winScreenCanvasGroup.SetGameObjectInactive();
            looseScreenCanvasGroup.SetGameObjectInactive();
        }

        private void OnDestroy()
        {
            _gameController.OnLevelEnded -= OnLevelEnded;
        }

        private void OnLevelEnded(bool isVictory)
        {
            if(isVictory)
                ShowWinScreen();
            else
                ShowLooseScreen();
        }

        private void ShowWinScreen()
        {
            winScreenCanvasGroup.alpha = 0;
            winScreenCanvasGroup.SetGameObjectActive();

            winScreenCanvasGroup.DOFade(1, animationDuration);
        }
        
        private void ShowLooseScreen()
        {
            looseScreenCanvasGroup.alpha = 0;
            looseScreenCanvasGroup.SetGameObjectActive();
            
            looseScreenCanvasGroup.DOFade(1, animationDuration);
        }
    }
}