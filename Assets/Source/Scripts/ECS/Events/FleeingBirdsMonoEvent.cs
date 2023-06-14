using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame.Events
{
    public sealed class FleeingBirdsMonoEvent : MonoBehaviour
    {
        [SerializeReference]
        private List<FleeingBirdWrapper> birds;

  

        private bool _wasTriggered;
        private void OnTriggerEnter(Collider other)
        {
            if(_wasTriggered)
                return;
            
            if(!other.transform.CompareTag("Player"))
                return;
            
            foreach (var bird in birds)
                bird.Flee();
            
            _wasTriggered = true;
        }
    }
}