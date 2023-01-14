using System;
using Ingame.SaveLoad;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace Ingame.Settings
{
    public sealed class GameSettingsService : MonoBehaviour
    {
        [SerializeField] private SettingsData defaultSettingsData;
        
        [ReadOnly, SerializeField] private SettingsData _currentSettingsData = new();
        private SaveLoadService _saveLoadService;

        private bool _isCursorEnabled;
        
        public SettingsData CurrentSettingsData => _currentSettingsData;

        public bool IsCursorEnabled
        {
            set
            {
                _isCursorEnabled = value;

                Cursor.visible = _isCursorEnabled;
                Cursor.lockState = _isCursorEnabled ? CursorLockMode.Confined : CursorLockMode.Locked;
            }
            get => _isCursorEnabled;
        }

        [Inject]
        private void Construct(SaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
            IsCursorEnabled = true;
        }

        private void Start()
        {
            if (_saveLoadService.HasData<SettingsData>()) 
                LoadSettingsFromSave();
            else
                ResetSettingsToDefault();
        }

        private void LoadSettingsFromSave()
        {
            var persistentData = _saveLoadService.GetData<SettingsData>();
            
            _currentSettingsData.CopyDataFrom(persistentData);
        }
        
        [Button]
        private void ResetSettingsToDefault()
        {
            _currentSettingsData.CopyDataFrom(defaultSettingsData);
        }

        [Button]
        public void ApplySettings()
        {
            Application.targetFrameRate = _currentSettingsData.maxFps;
        }
        
        public void SaveSettings()
        {
            _saveLoadService.SetData(_currentSettingsData);
            _saveLoadService.Save();
        }
    }
    
    [Serializable]
    public sealed class SettingsData
    {
        public int maxFps = 144;
        public float sensitivity = 1f;
        public float soundVolume = 100f;

        public void CopyDataFrom(SettingsData settingsData)
        {
            maxFps = settingsData.maxFps;
            sensitivity = settingsData.sensitivity;
            soundVolume = settingsData.soundVolume;
        }
    }
}