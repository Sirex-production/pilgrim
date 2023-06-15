using System;
using System.Collections.Generic;
using Ingame.Audio;
using UnityEngine;
using Zenject;

namespace Ingame.Events
{
    public sealed class FleeingBirdsMonoEvent : MonoBehaviour
    {
        [SerializeReference]
        private List<FleeingBirdWrapper> birds;

        private AudioService _audioService;
        private bool _wasTriggered;
        
        [Inject] 
        public void Construct(AudioService audioService)
        {
            _audioService = audioService;
        }
        
        private void Start()
        {
            _audioService.Play3D("town","crow",transform);
        }

 
        private void OnTriggerEnter(Collider other)
        {
            if(_wasTriggered)
                return;
            
            if(!other.transform.CompareTag("Player"))
                return;
            
            foreach (var bird in birds)
                bird.Flee();
            
            _wasTriggered = true;
            _audioService.Stop3D("town","crow",transform);
        }
    }
}