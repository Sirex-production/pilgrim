using UnityEngine;
using Zenject;

namespace Ingame.UI.MainMenu
{
	public sealed class MainMenuInstaller : MonoInstaller
	{
		[SerializeField] private MainMenuService mainMenuService;
		
		public override void InstallBindings()
		{
			Container.Bind<MainMenuService>()
				.FromInstance(mainMenuService)
				.AsSingle();
		}
	}
}