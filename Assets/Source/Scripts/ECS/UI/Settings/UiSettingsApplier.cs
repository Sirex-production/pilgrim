using Ingame.Settings;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Ingame.UI.Settings
{
    public sealed class UiSettingsApplier : MonoBehaviour
    {
        [BoxGroup("References (Common)")]
        [Required, SerializeField] private Button applyButton;
        [BoxGroup("References (Common)")]
        [Required, SerializeField] private Button closeButton;

        [BoxGroup("References (Controls)")]
        [Required, SerializeField] private Slider sensitivitySlider;
        [BoxGroup("References (Video)")]
        [Required, SerializeField] private TMP_Dropdown maximumFpsDropdown;
        [BoxGroup("References (Audio)")]
        [Required, SerializeField] private Slider soundVolumeSlider;

        private const int NO_FPS_LIMIT_DROPDOWN_OPTION_INDEX = 0;
        
        private GameSettingsService _gameSettingsService;
        private UiSettingsService _uiSettingsService;
        
        [Inject]
        private void Construct(GameSettingsService gameSettingsService, UiSettingsService uiSettingsService)
        {
            _gameSettingsService = gameSettingsService;
            _uiSettingsService = uiSettingsService;
        }

        private void Awake()
        {
            applyButton.onClick.AddListener(OnApplyButtonClicked);
            closeButton.onClick.AddListener(OnCloseButtonClicked);
            
            RecreateSettings();
        }

        private void OnDestroy()
        {
            applyButton.onClick.RemoveListener(OnApplyButtonClicked);
            closeButton.onClick.RemoveListener(OnCloseButtonClicked);
        }

        private void OnApplyButtonClicked()
        {
            ApplySettings();
        }

        private void OnCloseButtonClicked()
        {
            _uiSettingsService.RequestCloseSettingsWindow();
        }

        private void RecreateSettings()
        {
            var settingsData = _gameSettingsService.CurrentSettingsData;

            sensitivitySlider.value = settingsData.sensitivity;
            soundVolumeSlider.value = settingsData.soundVolume;
            
            RecreateMaxFpsView();
        }

        private void RecreateMaxFpsView()
        {
            var settingsData = _gameSettingsService.CurrentSettingsData;
            var maxFpsDropdownOptions = maximumFpsDropdown.options;
            
            for (int i = 0; i < maxFpsDropdownOptions.Count; i++)
            {
                if (int.TryParse(maxFpsDropdownOptions[i].text, out int parsedValue) && parsedValue == settingsData.maxFps)
                {
                    maximumFpsDropdown.value = i;
                    return;
                }
            }

            maximumFpsDropdown.value = NO_FPS_LIMIT_DROPDOWN_OPTION_INDEX;
        }

        public void ApplySettings()
        {
            var settingsData = _gameSettingsService.CurrentSettingsData;

            settingsData.sensitivity = sensitivitySlider.value;
            settingsData.maxFps = maximumFpsDropdown.value == NO_FPS_LIMIT_DROPDOWN_OPTION_INDEX
                ? -1
                : int.Parse(maximumFpsDropdown.options[maximumFpsDropdown.value].text);
            settingsData.soundVolume = soundVolumeSlider.value;
            
            _gameSettingsService.ApplySettings();
            _gameSettingsService.SaveSettings();
        }
    }
}