using System;
using NaughtyAttributes;
using Support;
using Support.Extensions;
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

        private ComicsService _comicsService;
        
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
            
            _comicsService.onClose += HideComics;
            _comicsService.onOpen += ExposeComics;
            _comicsService.onPageChanged += UpdateComics;
            
            comicsImage.SetGameObjectInactive();
            
        }
        
        private void OnDestroy()
        {
            _comicsService.onClose -= HideComics;
            _comicsService.onOpen -= ExposeComics;
            _comicsService.onPageChanged -= UpdateComics;
        }

        private void UpdateComics()
        {
            comicsImage.sprite = _comicsService.GetCurrentPage();
        }
        
        private void HideComics()
        {
            comicsImage.SetGameObjectInactive();
        }
        
        private void ExposeComics()
        {
            comicsImage.SetGameObjectActive();
        }
    }
}