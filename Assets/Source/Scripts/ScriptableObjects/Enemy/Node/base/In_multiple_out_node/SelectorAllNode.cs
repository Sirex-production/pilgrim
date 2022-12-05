namespace Ingame.Behaviour
{
    public class SelectorAllNode : CompositeNode
    {
        protected override void ActOnStart()
        {
             
        }

        protected override void ActOnStop()
        {
             
        }

        protected override State ActOnTick()
        {
            var status = State.Running;
            foreach (var child in Children)
            {
                switch (child.Tick()) {
                    case State.Running:
                        return State.Running;
                    case State.Success:
                        status = State.Success;
                        continue;
                    case State.Failure:
                        continue;
                    case State.Abandon:
                        return State.Abandon;
                }
            }

            return status;    
        }
    }
}