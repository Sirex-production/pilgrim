using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame.Behaviour
{
 
    public abstract class CompositeNode : Node
    {
        [SerializeField]
        [HideInInspector]
        public List<Node> Children = new();
        
        public override Node Clone()
        {
            var node = Instantiate(this);
            node.Children = Children.ConvertAll(e=>e.Clone());
            return node;
        }
        
    }
}