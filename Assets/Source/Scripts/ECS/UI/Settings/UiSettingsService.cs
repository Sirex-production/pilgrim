using System;
using UnityEngine;

namespace Ingame.UI.Settings
{
	public sealed class UiSettingsService : MonoBehaviour
	{
		public event Action<UiSettingsSectionType> OnSwitchSettingsSectionRequested;
		public event Action OnOpenSettingsWindowRequested;
		public event Action OnCloseSettingsWindowRequested;

		public void RequestSwitchSettingsSection(UiSettingsSectionType sectionType)
		{
			OnSwitchSettingsSectionRequested?.Invoke(sectionType);
		}

		public void RequestOpenSettingsWindow()
		{
			OnOpenSettingsWindowRequested?.Invoke();
		}

		public void RequestCloseSettingsWindow()
		{
			OnCloseSettingsWindowRequested?.Invoke();
		}
	}
}