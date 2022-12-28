using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Support
{
	/// <summary>
	/// Class that manages levels
	/// </summary>
	public sealed class LevelManagementService : MonoBehaviour
	{
		[SerializeField] private float delayBeforeStartLoadingLevel = 1f;
		[SerializeField] private float delayBeforeFinishLoadingLevel = 1f;
        
		public event Action OnLoadingStarted;
		public event Action<float> OnLoadingProgressUpdated;
		public event Action OnLoadingFinished;

		private bool _isLoadingLevel = false;
		
		/// <summary>
		/// Loads level
		/// </summary>
		/// <param name="sceneIndex">Level index that will be loaded</param>
		/// <exception cref="ArgumentException"></exception>
		public void LoadLevel(int sceneIndex)
		{
			if (sceneIndex < 0)
				throw new ArgumentException($"There is no level with such index \"{sceneIndex}\"");

			if (_isLoadingLevel)
			{
				TemplateUtils.SafeDebug("Level is already loading", LogType.Error);
				return;
			}

			StartCoroutine(LoadSceneRoutine(sceneIndex));
		}

		/// <summary>Restarts last level that was saved in progress(SaveLoadSystem)</summary>
		public void RestartLevel()
		{
			int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

			LoadLevel(currentSceneIndex);
		}

		private IEnumerator LoadSceneRoutine(int sceneIndex)
		{
			_isLoadingLevel = true;
			OnLoadingStarted?.Invoke();

			yield return new WaitForSeconds(delayBeforeStartLoadingLevel);
            
			var loadingAsyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
            
			while (!loadingAsyncOperation.isDone)
			{
				OnLoadingProgressUpdated?.Invoke(loadingAsyncOperation.progress);
				yield return null;
			}
			
			yield return new WaitForSeconds(delayBeforeFinishLoadingLevel);
			
			_isLoadingLevel = false;
			
			OnLoadingFinished?.Invoke();
		}
	}
}