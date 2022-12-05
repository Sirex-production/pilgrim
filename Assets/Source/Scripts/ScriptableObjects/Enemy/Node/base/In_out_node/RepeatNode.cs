using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


namespace Ingame.Behaviour
{
 
    public class RepeatNode : DecoratorNode
    {
        private enum TypeOfRepetition
        {
            Infinite, 
            Fixed,
            RestartOnState,
            RandomFixed
        }

        [SerializeField] private TypeOfRepetition type;
        
        //fixed
        [ShowIf("IsFixedType")]
        [SerializeField] 
        [Min(0)]
        private int loops = 1;
        
        //Restart On State
        [ShowIf("IsRestartOnState")]
        [SerializeField] private State stateOnResetState;

        [ShowIf("IsRandomFixedType")] 
        [SerializeField] 
        [Min(0)]
        private int minLoops;
        
        [ShowIf("IsRandomFixedType")] 
        [SerializeField] 
        [Min(0)]
        private int maxLoops;
        
        private int _currentLoop;
        protected override void ActOnStart()
        {
            _currentLoop = 0;
            
            if (type == TypeOfRepetition.RandomFixed)
            {
                loops = Random.Range(minLoops, maxLoops);
            }
        }

        protected override void ActOnStop()
        {
             
        }

        protected override State ActOnTick()
        {
            //infinite loop
            if (type == TypeOfRepetition.Infinite)
            {
                var state = Child.Tick();
                if (state == State.Abandon)
                {
                    Child.Abort();
                    return State.Abandon;
                }
                return State.Running;
            }
            //fixed or randomFixed
            if (type is TypeOfRepetition.Fixed or TypeOfRepetition.RandomFixed)
            {

                var state = State.Running;
                if (_currentLoop++ == loops)
                {
                    state = State.Success;
                }

                if ( Child.Tick() == State.Abandon)
                {
                    return State.Abandon;
                }

                if (state == State.Success)
                {
                    return State.Success;
                }
                return State.Running;
            }
            //ResetOnState
            if (type == TypeOfRepetition.RestartOnState)
            {
                var state = Child.Tick();
                if (state == this.stateOnResetState)
                {
                    return State.Running;
                }
                return state;
            }
            
            return State.Failure;
        }

        private bool IsFixedType => type == TypeOfRepetition.Fixed;
        private bool IsRestartOnState => type == TypeOfRepetition.RestartOnState;
        private bool IsRandomFixedType => type == TypeOfRepetition.RandomFixed;
    }
}