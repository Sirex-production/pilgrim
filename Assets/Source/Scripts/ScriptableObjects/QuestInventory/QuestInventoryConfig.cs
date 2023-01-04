using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame.QuestInventory
{
    public sealed class QuestInventoryConfig : ScriptableObject
    {
        [SerializeField] private List<PickableItem> items;
    }

    [Serializable]
    public sealed class PickableItem : ScriptableObject
    {
        [SerializeField] private GameObject gameObject;
        [SerializeField] private new string name ;

 


        public GameObject GameObject => gameObject;

        public string Name => name;
 
    }
}