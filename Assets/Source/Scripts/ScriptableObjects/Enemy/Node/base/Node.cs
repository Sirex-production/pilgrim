using System;
using System.Collections;
using System.Collections.Generic;
using Ingame.Audio;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ingame.Behaviour 
{
    [Serializable]
    public abstract class Node : ScriptableObject
    {
        public enum State
        {
           Running,
           Failure,
           Success,
           Abandon,
           //ForceStop
        }
        
        [FormerlySerializedAs("Guid")] [HideInInspector]
        public string guid;
        
        [FormerlySerializedAs("Position")] [HideInInspector]
        public Vector2 position = Vector2.zero;

        [TextArea] public string description;
        
        [HideInInspector]
        public AudioService audioService;
        
        public EcsEntity entity;
        public EcsWorld world;
        
        private bool _isRunning = false;
        private State _state = State.Running;
      
        public bool IsRunning
        {
            get => _isRunning;
            set => _isRunning = value;
        }

        public State CurrentState => _state;
        
        public State Tick()
        {
            
            if (!_isRunning)
            {
                ActOnStart();
                _isRunning = true;
            }

            _state = ActOnTick();
            if (_state is State.Success or State.Failure or State.Abandon)
            {
                ActOnStop();
                _isRunning = false;
            }

            return _state;
        }

        public virtual Node Clone()
        {
            return Instantiate(this);
        }
        protected abstract void ActOnStart();
        protected abstract void ActOnStop();
        protected abstract State ActOnTick();

        public void RequestToStop()
        {
            ActOnStop();
        }
        public void Abort(bool isForceful = false) {
            BehaviourTree.Traverse(this, (node) => {
                node.IsRunning = false;
                node._state = State.Running;
                node.RequestToStop();
                _state = isForceful ? State.Abandon : _state;
            });
        }

        public virtual void RestartState()
        {
            ActOnStart();
        }
    }
}