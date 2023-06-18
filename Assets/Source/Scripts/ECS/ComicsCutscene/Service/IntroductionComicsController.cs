using System;
using Ingame.Audio;
using NaughtyAttributes;
using Support;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Ingame.Comics
{
    public sealed class IntroductionComicsController : MonoBehaviour
    {
        [SerializeField] 
        [Required]
        private UiComicsViewController uiComicsViewController;

        [SerializeField] 
        private string nameOfComics;

        [SerializeField] 
        [Scene] 
        private int indexOfScene; 
        
        private StationaryInput _input;
        private ComicsService _comicsService;
        private LevelManagementService _levelManagementService;

        [Inject]
        private void Construct(StationaryInput input, ComicsService comicsService, LevelManagementService levelManagementService)
        {
            _input = input;
            _comicsService = comicsService;
            _levelManagementService = levelManagementService;
        }
        
        private void Awake()
        {
            _input.Comics.Enable();
            
            _input.Comics.Next.performed -= PerformNextPageLogic;
            _input.Comics.Back.performed -= PerformBackPageLogic;
            // _input.Comics.Skip.performed -= PerformSkipPageLogic;
            _comicsService.OnComicsClosed -= OnComicsClosed;
            
            _input.Comics.Next.performed += PerformNextPageLogic;
            _input.Comics.Back.performed += PerformBackPageLogic;
            // _input.Comics.Skip.performed += PerformSkipPageLogic;
            _comicsService.OnComicsClosed += OnComicsClosed;

        }

        private void Start()
        {
            _comicsService.Play(nameOfComics);
        }
        
        private void OnDestroy()
        {
            _input.Comics.Next.performed -= PerformNextPageLogic;
            _input.Comics.Back.performed -= PerformBackPageLogic;
            // _input.Comics.Skip.performed -= PerformSkipPageLogic;
            _comicsService.OnComicsClosed -= OnComicsClosed;
            
            _input.Comics.Disable();
            
        }

        private void PerformNextPageLogic(InputAction.CallbackContext callback)
        {
            if (uiComicsViewController.UseVideo && uiComicsViewController.IsVideoPlayerRunning)
            {
                uiComicsViewController.FinishVideoClip();
                return;
            }
            
            if(!uiComicsViewController.UseVideo && uiComicsViewController.TryToSpeedUpWriting())
               return;
            
            _comicsService.OpenNextPage();
        }
        private void PerformBackPageLogic(InputAction.CallbackContext callback)
        {
            _comicsService.OpenPreviousPage();
        }
        private void PerformSkipPageLogic(InputAction.CallbackContext callback)
        {
            _comicsService.Close();
        }

        private void OnComicsClosed()
        {
            _levelManagementService.LoadLevel(indexOfScene);
        }
    }
}