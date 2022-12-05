using UnityEngine;

namespace Ingame.Utils
{
    public interface ITriggerMessageSender
    {
        public void SendMessageOnEnter(Collider other);
        public void SendMessageOnExit(Collider other);
    }
}