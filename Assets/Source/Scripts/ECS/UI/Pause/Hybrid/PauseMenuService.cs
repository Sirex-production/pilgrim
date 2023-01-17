using System;
using UnityEngine;

namespace Ingame.UI.Pause
{
    public sealed class PauseMenuService : MonoBehaviour
    {
        public event Action OnPauseMenuShowRequested;
        public event Action OnPauseMenuHideRequested;
        
        private bool _isOpened = false;

        public bool IsOpened => _isOpened;

        public void RequestToShowPauseMenu()
        {
            OnPauseMenuShowRequested?.Invoke();
            _isOpened = true;
        }
        
        public void RequestToHidePauseMenu()
        {
            OnPauseMenuHideRequested?.Invoke();
            _isOpened = false;
        }
    }
}