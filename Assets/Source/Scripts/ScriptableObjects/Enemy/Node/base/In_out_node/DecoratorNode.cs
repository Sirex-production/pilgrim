using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame.Behaviour
{
 
    public abstract class DecoratorNode : Node
    {
        [SerializeField]
        private Node child;

        public Node Child
        {
            get => child;
            set => child = value;
        }
        
        public override Node Clone()
        {
            var node = Instantiate(this);
            node.child = child.Clone();
            return node;
        }
    }
}