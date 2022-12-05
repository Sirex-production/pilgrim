using Leopotam.Ecs;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace Ingame.Utils.MessageSenders
{
    public sealed class UpdateSettingsTriggerMessageSender : MonoBehaviour, ITriggerMessageSender
    {
        [BoxGroup("OnTriggerEnter")] 
        [SerializeField] private UpdateSettingsRequest settingsRequestOnEnter;
        [BoxGroup("OnTriggerExit")]
        [SerializeField] private UpdateSettingsRequest settingsRequestOnExit;

        [Inject] private readonly EcsWorld _world;

        public void SendMessageOnEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                _world.NewEntity().Get<UpdateSettingsRequest>() = settingsRequestOnEnter;
        }

        public void SendMessageOnExit(Collider other)
        {
            if (other.CompareTag("Player"))
                _world.NewEntity().Get<UpdateSettingsRequest>() = settingsRequestOnExit;
        }
    }
}