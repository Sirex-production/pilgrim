using Support;
using Voody.UniLeo;
using Zenject;

namespace Ingame.UI.Pause
{
	public sealed class PauseMenuModelProvider : MonoProvider<PauseMenuServiceModel>
	{
		[Inject]
		private void Construct(PauseMenuService pauseMenuService)
		{
			if (pauseMenuService == null)
			{
				TemplateUtils.SafeDebug($"{nameof(PauseMenuService)} is not injected into {nameof(PauseMenuModelProvider)}");
				return;
			}

			value = new PauseMenuServiceModel
			{
				pauseMenuService = pauseMenuService
			};
		}
	}
}