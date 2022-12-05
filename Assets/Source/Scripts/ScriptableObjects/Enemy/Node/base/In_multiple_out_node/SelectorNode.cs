using System.Collections;
using System.Collections.Generic;
using Ingame.Behaviour;
using UnityEngine;

namespace Ingame.Behaviour
{
    public class SelectorNode : CompositeNode
    {
        protected int currentIndex;
        
        protected override void ActOnStart()
        {
            currentIndex = 0;
        }

        protected override void ActOnStop()
        {
       
        }

        protected override State ActOnTick()
        {
            for (int i = currentIndex; i < Children.Count; i++)
            {
                currentIndex = i;
                var child = Children[currentIndex];
                
                switch (child.Tick()) {
                    case State.Running:
                        return State.Running;
                    case State.Success:
                        return State.Success;
                    case State.Failure:
                        continue;
                    case State.Abandon:
                        return State.Abandon;
                }
            }

            return State.Failure;
        }
    }
}