using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame.Behaviour
{
    public class TimeOutNode : DecoratorNode
    {
        [SerializeField]
        protected float duration = 1f;
        private float _timer = 0;
        protected override void ActOnStart()
        {
            _timer = duration;
        }

        protected override void ActOnStop()
        {
            
        }

        protected override State ActOnTick()
        {
            if (_timer<=0f)
            {
                if (Child.Tick()== State.Running)
                {
                    Child.Abort();
                }
                return State.Success;
            }

            Child.Tick();
            _timer -= Time.deltaTime;
            return State.Running;
        }
    }    
}
