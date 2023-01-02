using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace Ingame.UI.Pause
{
	public sealed class PauseMenuInstaller : MonoInstaller
	{
		[Required, SerializeField] private PauseMenuService pauseMenuService;
		
		public override void InstallBindings()
		{
			Container.Bind<PauseMenuService>()
				.FromInstance(pauseMenuService)
				.AsSingle();
		}
	}
}