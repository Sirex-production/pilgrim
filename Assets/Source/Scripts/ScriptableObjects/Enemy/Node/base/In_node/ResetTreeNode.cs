namespace  Ingame.Behaviour
{
    public class ResetTreeNode : ActionNode
    {
        protected override void ActOnStart()
        {
        
        }

        protected override void ActOnStop()
        {
             
        }

        protected override State ActOnTick()
        {
            return State.Abandon;
        }
    }
}