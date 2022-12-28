using System;
using JetBrains.Annotations;

namespace Ingame.Behaviour
{
    public class InterruptSelectorNodeNonBlockOnRunning : SelectorNode
    {
        
        protected override State ActOnTick()
        {
            var previous = currentIndex;
            base.ActOnStart();
            var status = base.ActOnTick();
            if (previous == currentIndex) return status;
            Children[previous].Tick();
            if (Children[previous].CurrentState == State.Running && status == State.Success)
            {
                Children[previous].Abort();
            }
            currentIndex = previous;
            return status;
        }
    }
}