using Ingame.Behaviour;
using UnityEngine;

namespace Ingame.Enemy
{
    public class PlayAnimationActionNode : ActionNode
    {
        [SerializeField] private string animationName;
        protected override void ActOnStart()
        {
             
        }

        protected override void ActOnStop()
        {
            
        }

        protected override State ActOnTick()
        {
            return State.Success;
        }
    }
}