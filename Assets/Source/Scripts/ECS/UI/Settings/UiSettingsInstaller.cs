using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace Ingame.UI.Settings
{
	public sealed class UiSettingsInstaller : MonoInstaller
	{
		[Required, SerializeField] private UiSettingsService uiSettingsService;
		
		public override void InstallBindings()
		{
			Container.Bind<UiSettingsService>()
				.FromInstance(uiSettingsService)
				.AsSingle();
		}
	}
}