using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Ingame.Behaviour
{
    public sealed class ParallelNode : CompositeNode
    {
        private Dictionary<Node,State> _childrenState = new ();
        protected override void ActOnStart()
        {
            _childrenState = new ();
            Children.ForEach(e=>_childrenState.Add(e,State.Running));
        }

        protected override void ActOnStop()
        {
           
        }

        protected override State ActOnTick()
        {
            var hasFinished = true;
            //todo change a cloning
            var dir = _childrenState.ToDictionary(e=> e.Key,e=>e.Value);
            foreach (var item in dir)
            {
                if (item.Value != State.Running)
                {
                    continue;
                }

                var state = item.Key.Tick();
                if (state is State.Failure or State.Abandon)
                {
                    AbortRunningChildren();
                    return State.Failure;
                }

                if (state == State.Running)
                {
                    hasFinished = false;
                }

                _childrenState[item.Key] = state;

            }

            return hasFinished ? State.Success : State.Running;
        }
        void AbortRunningChildren()
        {
            foreach (var item in _childrenState.Where(item => item.Value == State.Running))
            {
                item.Key.Abort();
            }
        }
    }
}