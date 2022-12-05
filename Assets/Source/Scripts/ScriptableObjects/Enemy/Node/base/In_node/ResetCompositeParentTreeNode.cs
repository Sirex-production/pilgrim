 using UnityEngine;

 namespace Ingame.Behaviour
{
    public class ResetCompositeParentTreeNode : ActionNode
    {
        [SerializeField] 
        private CompositeNode compositeParentNode;
        protected override void ActOnStart()
        {
             
        }

        protected override void ActOnStop()
        {
        
        }

        protected override State ActOnTick()
        {
            compositeParentNode.RestartState();
            return State.Success;
        }
    }
}