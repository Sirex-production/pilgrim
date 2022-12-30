using System;
using Support;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Ingame.Comics
{
    public class IntroductionComicsController : MonoBehaviour
    {

        [SerializeField] 
        private string nameOfComics;

        [SerializeField] 
        [Min(0)] 
        private int indexOfScene; 
        
        private StationaryInput _input;
        private ComicsService _comicsService;
        private LevelManagementService _levelManagementService;

        [Inject]
        private void Constructor(StationaryInput input, ComicsService comicsService, LevelManagementService levelManagementService)
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
            _input.Comics.Skip.performed -= PerformSkipPageLogic;
            _comicsService.onClose -= FinishComics;
            
            _input.Comics.Next.performed += PerformNextPageLogic;
            _input.Comics.Back.performed += PerformBackPageLogic;
            _input.Comics.Skip.performed += PerformSkipPageLogic;
            _comicsService.onClose += FinishComics;
        }

        private void Start()
        {
           _comicsService.Play(nameOfComics);
        }

        private void OnDestroy()
        {
            _input.Comics.Next.performed -= PerformNextPageLogic;
            _input.Comics.Back.performed -= PerformBackPageLogic;
            _input.Comics.Skip.performed -= PerformSkipPageLogic;
            _comicsService.onClose -= FinishComics;
            
            _input.Comics.Disable();
        }

        private void PerformNextPageLogic(InputAction.CallbackContext callback)
        {
            _comicsService.Next();
        }
        private void PerformBackPageLogic(InputAction.CallbackContext callback)
        {
            _comicsService.Back();
        }
        private void PerformSkipPageLogic(InputAction.CallbackContext callback)
        {
            _comicsService.Skip();
        }

        private void FinishComics()
        {
            _levelManagementService.LoadLevel(indexOfScene);
        }
    }
}