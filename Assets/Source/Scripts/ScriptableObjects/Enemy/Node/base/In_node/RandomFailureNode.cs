using UnityEngine;

namespace Ingame.Behaviour
{
    public class RandomFailureNode : ActionNode
    {
        [SerializeField] 
        [Range(0,1)]
        private float failureRate = .3f;
        protected override void ActOnStart()
        {
             
        }

        protected override void ActOnStop()
        {
         
        }

        protected override State ActOnTick()
        {
            return Random.Range(0f, 1f) >= failureRate ? State.Success : State.Failure;
        }
    }
}