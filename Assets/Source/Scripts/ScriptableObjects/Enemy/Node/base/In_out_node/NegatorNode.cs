 
namespace Ingame.Behaviour
{
    public class NegatorNode : DecoratorNode
    {
        protected override void ActOnStart()
        {
            
        }

        protected override void ActOnStop()
        {
           
        }

        protected override State ActOnTick()
        {
            var state = Child.Tick();

            switch (state)
            {
                case State.Success:
                    return State.Failure;
                case State.Failure: 
                    return State.Success;
                case State.Running :
                    return State.Running;
            }

            return state;
        }
    }
}