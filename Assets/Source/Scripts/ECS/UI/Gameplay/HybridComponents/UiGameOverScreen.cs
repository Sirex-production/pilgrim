using System;
using DG.Tweening;
using Ingame.Settings;
using NaughtyAttributes;
using Support;
using Support.Extensions;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Ingame.UI
{
    public sealed class UiGameOverScreen : MonoBehaviour
    {
        [BoxGroup("References"), Required, SerializeField] private RectTransform looseScreenParent;
        [BoxGroup("References"), Required, SerializeField] private Button restartButton;
        
        [BoxGroup("Settings"), SerializeField] [Min(0)] private float showAnimationDuration;
        
        private LevelManagementService _levelManagementService;
        private GameSettingsService _gameSettingsService;

        [Inject]
        private void Construct(LevelManagementService levelManagementService, GameSettingsService gameSettingsService)
        {
            _levelManagementService = levelManagementService;
            _gameSettingsService = gameSettingsService;
        }

        private void Awake()
        {
            restartButton.onClick.AddListener(OnRestartButtonClicked);
            
            looseScreenParent.SetGameObjectInactive();
            restartButton.SetGameObjectInactive();
        }

        private void OnDestroy()
        {
            restartButton.onClick.RemoveListener(OnRestartButtonClicked);
        }

        private void OnRestartButtonClicked()
        {
            _levelManagementService.RestartLevel();
        }

        public void ShowGameOverScreen()
        {
            looseScreenParent.transform.localScale = Vector3.zero;
            restartButton.transform.localScale = Vector3.zero;
            
            looseScreenParent.SetGameObjectActive();
            restartButton.SetGameObjectActive();

            _gameSettingsService.IsCursorEnabled = true;
            
            DOTween.Sequence()
                .Append(looseScreenParent.DOScale(Vector3.one, showAnimationDuration))
                .Append(restartButton.transform.DOScale(Vector3.one, showAnimationDuration))
                .Play();
        }
    }
}