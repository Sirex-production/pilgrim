using UnityEngine;

namespace Ingame.QuestInventory
{
    public class DebugItem : UsableItem
    {
        [SerializeField] private string message = "Hello World!";
        public override void Use()
        {
            #if UNITY_EDITOR
                UnityEngine.Debug.Log(message);    
            #endif
        }
    }
}