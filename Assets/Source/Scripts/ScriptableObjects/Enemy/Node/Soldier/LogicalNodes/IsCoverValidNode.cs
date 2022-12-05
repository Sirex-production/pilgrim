using Ingame.Behaviour;

namespace Ingame.Enemy
{
    public class IsCoverValidNode : ActionNode
    {
        protected override void ActOnStart()
        {
             
        }

        protected override void ActOnStop()
        { 
        }

        protected override State ActOnTick()
        {
            return State.Failure;
        }
    }
}