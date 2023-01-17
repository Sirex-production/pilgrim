using System;
using UnityEngine;
using Zenject;

namespace Ingame.Audio
{
    public sealed class PlayAudioOnStart : MonoBehaviour
    {
        
        [SerializeField] 
        private string audioType = "";
        
        [SerializeField] 
        private string audioName = "";

        [SerializeField] private bool is3DSound;
        
        private AudioService _audioService;

        [Inject]
        private void Construct(AudioService audioService)
        {
            _audioService = audioService;
        }
        
        private void Start()
        {
            if (is3DSound)
            {
                _audioService.Play3D(audioType,audioName,transform);
            }
            else
            {
                _audioService.Play(audioType,audioName);
            }
        }
    }
}