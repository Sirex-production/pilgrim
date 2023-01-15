using System;
using DG.Tweening;
using NaughtyAttributes;
using Support;
using Support.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Ingame.Comics
{
    public sealed class UiComicsViewController : MonoBehaviour
    {
        [SerializeField]
        [Required]
        private Image comicsImage;

        [SerializeField]
        [Required]
        private TextMeshProUGUI comicsText;
        
        [SerializeField]
        [Min(0)]
        private float frequencyOfLettersAppearance = 0.08f;
        
        private ComicsService _comicsService;
        private Tween _tweenTextWriter;
        private string _currentText = "";
        private bool _isDoTweenOccupied = false;
        private string _comicsCurrentText = "";
        
        [Inject]
        private void Construct(ComicsService comicsService )
        {
            _comicsService = comicsService;
        }

        private void Awake()
        {
            _comicsService.OnComicsClosed -= OnComicsClosed;
            _comicsService.OnComicsOpened -= OnComicsOpened;
            _comicsService.OnComicsPageChanged -= OnComicsPageChanged;
            _comicsService.OnComicsTextChanged -= OnComicsTextChanged;
            
            _comicsService.OnComicsClosed += OnComicsClosed;
            _comicsService.OnComicsOpened += OnComicsOpened;
            _comicsService.OnComicsPageChanged += OnComicsPageChanged;
            _comicsService.OnComicsTextChanged += OnComicsTextChanged;
            
            comicsImage.SetGameObjectInactive();
            
        }
        
        private void OnDestroy()
        {
            _comicsService.OnComicsClosed -= OnComicsClosed;
            _comicsService.OnComicsOpened -= OnComicsOpened;
            _comicsService.OnComicsPageChanged -= OnComicsPageChanged;
            _comicsService.OnComicsTextChanged -= OnComicsTextChanged;
        }

        private void OnComicsPageChanged(Sprite sprite)
        {
            comicsImage.sprite = sprite;
        }
        
        private void OnComicsTextChanged(string text)
        {
            _tweenTextWriter?.Kill();

            _comicsCurrentText = text;
            _currentText = "";
            _isDoTweenOccupied = true;
            
            _tweenTextWriter = DOTween
                .To(() => _currentText, text => _currentText = text, _comicsCurrentText,_comicsCurrentText.Length*frequencyOfLettersAppearance)
                .OnUpdate(() => comicsText.SetText(_currentText))
                .OnComplete(() => _isDoTweenOccupied = false);
        }
        
        private void OnComicsClosed()
        {
            comicsImage.SetGameObjectInactive();
        }
        
        private void OnComicsOpened()
        {
            comicsImage.SetGameObjectActive();
        }

        public bool TryToSpeedUpWriting()
        {
            if (!_isDoTweenOccupied) 
                return false;
            
            _tweenTextWriter?.Kill();
            comicsText.SetText(_comicsCurrentText);
            _isDoTweenOccupied = false;
                
            return true;

        }
    }
}