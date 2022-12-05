using System;
using UnityEngine;

namespace Support.UI
{
    /// <summary>
    /// Class that allows to access UI elements
    /// </summary>
    public class UiController : MonoBehaviour
    {
        public event Action OnUiRestartLevelTransition;
        public event Action OnUiLoadNextLevelTransition;

        public void PlayUiRestartLevelTransition()
        {
            OnUiRestartLevelTransition?.Invoke();
        }
        
        public void PlayUiLoadNextLevelTransition()
        {
            OnUiLoadNextLevelTransition?.Invoke();
        }
    }
}