using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Ingame.ECS{
    [Serializable]
    public struct UiComicsViewModel 
    {
        [SerializeField] 
        [Required]
        private Button skipButton;
        
        [SerializeField] 
        [Required]
        private Button backButton;
        
        [SerializeField] 
        [Required]
        private Button nextButton;
        
        [SerializeField] 
        [Required]
        private Image comicsBackgroundImage;


        public Button SkipButton => skipButton;

        public Button BackButton => backButton;

        public Button NextButton => nextButton;

        public Image ComicsBackgroundImage => comicsBackgroundImage;
    }
}