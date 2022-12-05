using System.Collections;
using System.Collections.Generic;
using Ingame.Enemy;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ingame.Behaviour
{
    public sealed class RootNode : Node
    {
        [SerializeField] private Node child;
        public Node Child
        {
            get => child;
            set => child = value;
        }

        protected override void ActOnStart()
        {
             
        }

        protected override void ActOnStop()
        {
            
        }

        protected override State ActOnTick()
        {
            return child.Tick();
        }

        public override Node Clone()
        {
            var node = Instantiate(this);
            node.child = child.Clone();
            return node;
        }
    }
}