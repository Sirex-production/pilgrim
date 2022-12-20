using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ingame.ECS{
    public class UiComicsView : MonoBehaviour
    {
        [SerializeField] private Button skipButton;
        [SerializeField] private Button backButton;
        [SerializeField] private Button nextButton;

        private void Start()
        {
           skipButton.onClick.AddListener(OnSkip);
           nextButton.onClick.AddListener(OnNext);
           backButton.onClick.AddListener(OnBack);
        }
        
        private void OnSkip(){}
        private void OnNext(){}
        private void OnBack(){}
    }
}