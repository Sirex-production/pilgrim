using System;
using DG.Tweening;
using Ingame.Audio;
using UnityEngine;
using Zenject;

namespace Ingame.Events
{
    public sealed class AppearTrainMonoEvent : MonoBehaviour
    {
        [SerializeReference] private Transform train;
        [SerializeReference] private Transform destination;
       
        private AudioService _audioService;
        private bool _wasTriggered;
        [Inject] 
        public void Construct(AudioService audioService)
        {
            _audioService = audioService;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if(_wasTriggered)
                return;
            
            if(!other.transform.CompareTag("Player"))
                return;

            _audioService.Play("town","train");
            train.DOMove(destination.position, 28);
            _wasTriggered = true;
        }
    }
}