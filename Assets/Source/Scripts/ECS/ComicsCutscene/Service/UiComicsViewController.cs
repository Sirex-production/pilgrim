using NaughtyAttributes;
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
        private void Constructor(ComicsService comicsService)
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
            
            comicsImage.gameObject.SetActive(false);
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
            comicsImage.gameObject.SetActive(false);
        }
        
        private void ExposeComics()
        {
            comicsImage.gameObject.SetActive(true);
        }
    }
}