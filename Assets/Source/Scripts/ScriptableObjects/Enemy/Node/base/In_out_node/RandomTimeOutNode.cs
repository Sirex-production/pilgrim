using UnityEngine;

namespace Ingame.Behaviour
{
    public class RandomTimeOutNode : TimeOutNode
    {
        [SerializeField] private float minTime  = .5f;
        [SerializeField] private float maxTime = 2f;
        protected override void ActOnStart()
        {
            duration = Random.Range(minTime, maxTime);
            base.ActOnStart();
        }
    }
}