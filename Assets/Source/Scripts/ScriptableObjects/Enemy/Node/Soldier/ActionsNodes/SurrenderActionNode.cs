using Ingame.Behaviour;

namespace Ingame.Enemy
{
    public class SurrenderActionNode : ActionNode
    {
        protected override void ActOnStart()
        {
             
        }

        protected override void ActOnStop()
        {
            
        }

        protected override State ActOnTick()
        {
            return State.Running;
        }
    }
}