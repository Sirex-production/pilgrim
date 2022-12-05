using UnityEngine;

namespace Ingame.Behaviour
{
    public class RandomWaitNode : ActionNode
    {
        [SerializeField] 
        [Min(0)]
        private float minTime = .3f;
        
        [SerializeField] 
        [Min(0)]
        private float maxTime = .9f;
        
        private float _timer;
        
        protected override void ActOnStart()
        {
            _timer = Random.Range(minTime, maxTime);
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