using System;
using UnityEngine;
using Zenject;

namespace Support
{
    /// <summary>
    /// Class that manages general game logic
    /// </summary>
    public class GameController : MonoBehaviour
    {
        /// <summary>Event that invokes each time when level is ended</summary>
        public event Action<bool> OnLevelEnded;

        private bool _isLevelEnded = false;

        /// <summary>
        /// Method that should be invoked when level is ended
        /// </summary>
        /// <param name="isVictory">Describes whether player won or not</param>
        public void EndLevel(bool isVictory)
        {
            if(_isLevelEnded)
                return;

            _isLevelEnded = true;
            
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            
            OnLevelEnded?.Invoke(isVictory);
        }
    }
}