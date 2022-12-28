using DG.Tweening;
using NaughtyAttributes;
using Support.Extensions;
using UnityEngine;
using Zenject;

namespace Ingame.UI.Settings
{
	public sealed class UiSettingsSectionSwitcher : MonoBehaviour
	{
		[BoxGroup("References")]
		[SerializeField] private Transform[] settingsSections;
		[BoxGroup("References")]
		[Required, SerializeField] private Transform settingsParentTransform;

		[BoxGroup("Animation properties")]
		[SerializeField] [Min(0f)] private float scaleAnimationDuration = .3f;

		private UiSettingsService _uiSettingsService;
		
		private UiSettingsSectionType _currentSettingsSectionType = UiSettingsSectionType.None;
		private Sequence _showAnimationSequence;

		[Inject]
		private void Construct(UiSettingsService uiSettingsService)
		{
			_uiSettingsService = uiSettingsService;
		}

		private void Awake()
		{
			SwitchSection(UiSettingsSectionType.Controls);
			
			settingsParentTransform.localScale = new Vector3(1, 0, 1);
			settingsParentTransform.SetGameObjectInactive();

			_uiSettingsService.OnSwitchSettingsSectionRequested += SwitchSection;
			_uiSettingsService.OnOpenSettingsWindowRequested += OpenSettings;
			_uiSettingsService.OnCloseSettingsWindowRequested += CloseSettings;
		}

		private void OnDestroy()
		{
			_uiSettingsService.OnSwitchSettingsSectionRequested -= SwitchSection;
			_uiSettingsService.OnOpenSettingsWindowRequested -= CloseSettings;
			_uiSettingsService.OnCloseSettingsWindowRequested -= CloseSettings;
		}

		private void SwitchSection(UiSettingsSectionType uiSettingsSectionType)
		{
			if(_currentSettingsSectionType == uiSettingsSectionType)
				return;

			_currentSettingsSectionType = uiSettingsSectionType;
			
			if(_showAnimationSequence != null)
				_showAnimationSequence.Kill();
			
			_showAnimationSequence = DOTween.Sequence().Pause();

			foreach (var sectionTransform in settingsSections)
				_showAnimationSequence.Join(sectionTransform
					.DOScaleY(0, scaleAnimationDuration)
					.OnComplete(sectionTransform.SetGameObjectInactive));

			var targetSettingsSection = settingsSections[(int) _currentSettingsSectionType];
			
			_showAnimationSequence.Append(targetSettingsSection
				.DOScaleY(1, scaleAnimationDuration)
				.OnStart(targetSettingsSection.SetGameObjectActive));
			_showAnimationSequence.Play();
		}

		private void OpenSettings()
		{
			settingsParentTransform.SetGameObjectActive();
			settingsParentTransform.DOScaleY(1, scaleAnimationDuration)
				.SetEase(Ease.OutBack);
		}

		private void CloseSettings()
		{
			settingsParentTransform.DOScaleY(0, scaleAnimationDuration)
				.OnComplete(settingsParentTransform.SetGameObjectInactive);
		}
	}
}