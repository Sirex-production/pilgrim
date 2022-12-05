

using UnityEngine;

namespace Ingame.Behaviour
{
    public class RandomSelectorNode : CompositeNode
    {
        private int _randomNumber;
        protected override void ActOnStart()
        {
            _randomNumber = Random.Range(0, Children.Count);
        }

        protected override void ActOnStop()
        {
            
        }

        protected override State ActOnTick()
        {
            return Children[_randomNumber].Tick();
        }
    }    
}