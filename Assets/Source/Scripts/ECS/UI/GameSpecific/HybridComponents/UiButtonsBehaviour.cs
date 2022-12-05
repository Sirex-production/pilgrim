using Support.UI;
using UnityEngine;
using Zenject;

namespace Ingame.UI
{
    public class UiButtonsBehaviour : MonoBehaviour
    {
        [Inject] private readonly UiController _uiController;
        
        public void RestartLevel()
        {
            _uiController.PlayUiRestartLevelTransition();
        }
        
        public void LoadNextLevel()
        {
            _uiController.PlayUiLoadNextLevelTransition();
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}