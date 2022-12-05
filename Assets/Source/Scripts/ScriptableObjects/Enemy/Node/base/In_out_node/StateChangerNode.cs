using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame.Behaviour{
    public class StateChangerNode : DecoratorNode
    {
        [Space]
        [SerializeField]
        private List<NodeToChange> nodesToChange = new(); 
        protected override void ActOnStart()
        {
            
        }

        protected override void ActOnStop()
        {
            
        }

        protected override State ActOnTick()
        {
            var state = Child.Tick();
            foreach (var stateToChange in nodesToChange)
            {
                if (stateToChange.OldState == state)
                    return stateToChange.NewState;
            }
            return state;
        }
    }
    [Serializable]
    public class NodeToChange
    {
        [Space]
        [SerializeField] private Node.State oldState;
        [Space]
        [SerializeField] private Node.State newState;

        public Node.State OldState => oldState;

        public Node.State NewState => newState;
    }
}