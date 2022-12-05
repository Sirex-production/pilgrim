namespace Ingame.Behaviour
{
    public class SuccessNode : DecoratorNode
    {
        protected override void ActOnStart()
        {
             
        }

        protected override void ActOnStop()
        {
           
        }

        protected override State ActOnTick()
        {
            Child.Tick();
            return State.Success;
        }
    }
}