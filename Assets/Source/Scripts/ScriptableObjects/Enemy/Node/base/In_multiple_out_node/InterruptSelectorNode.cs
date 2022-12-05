namespace Ingame.Behaviour
{
    public sealed class InterruptSelectorNode : SelectorNode
    {
        protected override State ActOnTick()
        {
            var previous = currentIndex;
            base.ActOnStart();
            var status = base.ActOnTick();
            if (previous == currentIndex) return status;
            
            if (Children[previous].CurrentState == State.Running)
            {
                Children[previous].Abort();
            }
            
            return status;
        }
    }
}