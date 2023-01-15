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
        
        private ComicsService _comicsService;
        private Tween _tweenTextWriter;
        private string _currentText = "";
        private float _frequencyOfLettersAppearance = 0.08f;
        private bool _isDoTweenOccupied = false;
        private string _comicsCurrentText = "";
        [Inject]
        private void Construct(ComicsService comicsService )
        {
            _comicsService = comicsService;
        }

        private void Awake()
        {
            _comicsService.onClose -= HideComics;
            _comicsService.onOpen -= ExposeComics;
            _comicsService.onPageChanged -= UpdateComics;
            _comicsService.onTextChanged -= UpdateText;
            
            _comicsService.onClose += HideComics;
            _comicsService.onOpen += ExposeComics;
            _comicsService.onPageChanged += UpdateComics;
            _comicsService.onTextChanged += UpdateText;
            
            comicsImage.SetGameObjectInactive();
            
        }
        
        private void OnDestroy()
        {
            _comicsService.onClose -= HideComics;
            _comicsService.onOpen -= ExposeComics;
            _comicsService.onPageChanged -= UpdateComics;
            _comicsService.onTextChanged -= UpdateText;
        }

        private void UpdateComics()
        {
            comicsImage.sprite = _comicsService.GetCurrentPage();
        }
        
        private void UpdateText()
        {
            _tweenTextWriter?.Kill();

            _comicsCurrentText = _comicsService.GetCurrentText();
            _currentText = "";
            _isDoTweenOccupied = true;
            
            _tweenTextWriter = DOTween
                .To(() => _currentText, text => _currentText = text, _comicsCurrentText,_comicsCurrentText.Length*_frequencyOfLettersAppearance)
                .OnUpdate(() => comicsText.SetText(_currentText))
                .OnComplete(()=> _isDoTweenOccupied = false);
        }
        
        private void HideComics()
        {
            comicsImage.SetGameObjectInactive();
        }
        
        private void ExposeComics()
        {
            comicsImage.SetGameObjectActive();
        }

        public bool TryToSpeedUpWriting()
        {
            if (_isDoTweenOccupied)
            {
                _tweenTextWriter?.Kill();
                comicsText.SetText(_comicsCurrentText);
                _isDoTweenOccupied = false;
                
                return true;
            }

            return false;
        }
    }
}