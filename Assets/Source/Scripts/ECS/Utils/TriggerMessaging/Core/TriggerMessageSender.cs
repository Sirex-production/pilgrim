using System;
using System.Linq;
using ModestTree;
using UnityEngine;

namespace Ingame.Utils
{
    [RequireComponent(typeof(Collider))]
    public sealed class TriggerMessageSender : MonoBehaviour
    {
        private ITriggerMessageSender[] _messageSenders;

        private void Awake()
        {
            _messageSenders = GetComponents<ITriggerMessageSender>()?.Where(sender => sender != null).ToArray();
        }

        private void OnTriggerEnter(Collider other)
        {
            if(_messageSenders == null || _messageSenders.IsEmpty())
                return;
            
            foreach (var messageSender in _messageSenders)
            {
                messageSender.SendMessageOnEnter(other);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(_messageSenders == null || _messageSenders.IsEmpty())
                return;
            
            foreach (var messageSender in _messageSenders)
            {
                messageSender.SendMessageOnExit(other);
            }
        }
    }
}