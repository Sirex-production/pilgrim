using NaughtyAttributes;
using Support;
using UnityEngine;
using Zenject;

namespace Ingame.Debug
{
    public sealed class LevelManagementDebugger : MonoBehaviour
    {
        [Inject] private readonly LevelManager _levelManager;
        [Inject] private readonly GameController _gameController;

        [Button]
        private void RestartLevel()
        {
            _levelManager.RestartLevel();
        }

        [Button]
        private void LoadNextLevel()
        {
            _levelManager.LoadNextLevel();
        }

        [Button]
        private void EndLevelWithWin()
        {
            _gameController.EndLevel(true);
        }

        [Button]
        private void EndLevelWithLoose()
        {
            _gameController.EndLevel(false);
        }
    }
}