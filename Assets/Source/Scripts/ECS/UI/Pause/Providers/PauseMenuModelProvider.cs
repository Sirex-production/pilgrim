using NaughtyAttributes;
using Support;
using UnityEngine;
using Voody.UniLeo;
using Zenject;

namespace Ingame.UI.Pause
{
	public sealed class PauseMenuModelProvider : MonoProvider<PauseMenuServiceModel>
	{
		[Required, SerializeField] private PauseMenuService pauseMenuService; 
		
		[Inject]
		private void Construct()
		{
			if (pauseMenuService == null)
			{
				TemplateUtils.SafeDebug($"{nameof(PauseMenuService)} is null in {nameof(PauseMenuModelProvider)}", LogType.Error);
				return;
			}

			value = new PauseMenuServiceModel
			{
				pauseMenuService = pauseMenuService
			};
		}
	}
}