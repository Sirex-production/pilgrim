using System;
using JetBrains.Annotations;

namespace Ingame.Behaviour
{
    //async Interrupt Selector
    //todo make it work
    public class InterruptSelectorNodeNonBlockOnRunning : SelectorNode
    {
        
        protected override State ActOnTick()
        {
            var previous = currentIndex;
            base.ActOnStart();
            var status = base.ActOnTick();
            if (previous == currentIndex) return status;
            Children[previous].Tick();
            if (Children[previous].CurrentState == State.Running && status == State.Success)
            {
                Children[previous].Abort();
            }
            currentIndex = previous;
            return status;
        }
        /*
        private int _lastRememberedNodeIndex;
        private bool _isDeadLock = false;
        protected override State ActOnTick()
        {
            var previous = currentIndex;
            base.ActOnStart();
            var status = base.ActOnTick();
            if (previous == currentIndex && !_isDeadLock) return status;
            //is deadlock
            if (_isDeadLock)
            {
                var lastState = Children[_lastRememberedNodeIndex].Tick();
                //non abandon
                if (previous == currentIndex )
                {
                    if (status == State.Running && Children[_lastRememberedNodeIndex].CurrentState == State.Running)
                    {
                        return State.Running;
                    }

                    if (Children[_lastRememberedNodeIndex].CurrentState != State.Running)
                    {
                        return Children[previous].CurrentState;
                    }
                    return status;
                }
                
                
            }
            
            //both are not finished
            if (status == State.Running && Children[previous].CurrentState == State.Running && !_isDeadLock)
            {
                _lastRememberedNodeIndex = previous;
                _isDeadLock = true;
                return State.Running;
            }

            //current node is done
            if (Children[previous].CurrentState != State.Running)
            {
                return Children[previous].CurrentState;
            }
            return status;
        }
        protected override void ActOnStop()
      {
          base.ActOnStop();
          _isDeadLock = false;
          _lastRememberedNodeIndex = 0;
      }
      */
        
        /*private int _lastRememberedNodeIndex;
        private bool _isDeadLock = false;
        protected override State ActOnTick()
        {
            return _isDeadLock ? ActOnBlock() : ActOnNonBlock();
        }*/

        /*private State ActOnNonBlock()
        {
            var previous = currentIndex;
            base.ActOnStart();
            var status = base.ActOnTick();
            
            //check if should be interrupt
            if (previous == currentIndex) return status;

            if (status == State.Running)
            {
                _isDeadLock = true;
                _lastRememberedNodeIndex = previous;
                return status;
            }
            //abandon last state
            if (Children[previous].CurrentState == State.Running)
            {
                Children[previous].Abort();
            }
            
            return status; 
        }

        private State ActOnBlock()
        {
            var previous = currentIndex;
            base.ActOnStart();
            var status = base.ActOnTick();
            Children[_lastRememberedNodeIndex].Tick();
            
            //check if should be interrupt
            if (previous == currentIndex) return status;
            
            //abandon last state
            if (Children[previous].CurrentState == State.Running)
            {
                Children[_lastRememberedNodeIndex].Abort();
                Children[previous].Abort();
            }
            
            return status;
        }*/
        //block state if new state is Running     
        /*if (status == State.Running)
        {
            _lastRememberedNodeIndex = previous;
            _isDeadLock = true;
                
            return status;
        } */
        /*protected override void ActOnStop()
        {
            base.ActOnStop();
            _isDeadLock = false;
            _lastRememberedNodeIndex = 0;
        }*/
    }
}