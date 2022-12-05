using System.Timers;
using UnityEngine;

namespace Ingame.Behaviour
{
    public class WaitNode : ActionNode
    {
        [SerializeField] private float delayDuration = 1;

        private float _timer;
        protected override void ActOnStart()
        {
            _timer = delayDuration;
        }

        protected override void ActOnStop()
        {
       
        }

        protected override State ActOnTick()
        {
            if (_timer<=0f)
            {
                return State.Success;
            }
            _timer -= Time.deltaTime;
           
            return State.Running;
        }
    }
}