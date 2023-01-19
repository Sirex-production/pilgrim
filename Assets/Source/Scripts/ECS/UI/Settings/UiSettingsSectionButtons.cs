using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Ingame.UI.Settings
{
    public sealed class UiSettingsSectionButtons : MonoBehaviour
    {
        [BoxGroup("References")]
        [Required, SerializeField] private Button controlButtons;
        [BoxGroup("References")]
        [Required, SerializeField] private Button videoButtons;
        [BoxGroup("References")]
        [Required, SerializeField] private Button audioButtons;

        private UiSettingsService _uiSettingsService;
        
        [Inject]
        private void Construct(UiSettingsService uiSettingsService)
        {
            _uiSettingsService = uiSettingsService;
        }

        private void Awake()
        {
            controlButtons.onClick.AddListener(OnControlsButtonClicked);
            videoButtons.onClick.AddListener(OnVideoButtonClicked);
            audioButtons.onClick.AddListener(OnAudioButtonClicked);
        }

        private void OnDestroy()
        {
            controlButtons.onClick.RemoveListener(OnControlsButtonClicked);
            videoButtons.onClick.RemoveListener(OnVideoButtonClicked);
            audioButtons.onClick.RemoveListener(OnAudioButtonClicked);
        }

        private void OnControlsButtonClicked()
        {
            _uiSettingsService.RequestSwitchSettingsSection(UiSettingsSectionType.Controls);
        }
        
        private void OnVideoButtonClicked()
        {
            _uiSettingsService.RequestSwitchSettingsSection(UiSettingsSectionType.Video);
        }
        
        private void OnAudioButtonClicked()
        {
            _uiSettingsService.RequestSwitchSettingsSection(UiSettingsSectionType.Audio);
        }
    }
}