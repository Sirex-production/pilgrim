using System.Collections;
using System.Collections.Generic;
using Ingame.Behaviour;
using UnityEngine;

namespace Ingame.Behaviour.Test
{
    public class DebugActionNode : ActionNode
    {
        [SerializeField] private string message = "Bye Bye world!!!";
        protected override void ActOnStart()
        {
           UnityEngine.Debug.Log($"Started:{message}");
        }

        protected override void ActOnStop()
        {
            UnityEngine.Debug.Log($"Stopped:{message}");
        }

        protected override State ActOnTick()
        {
            UnityEngine.Debug.Log($"Tick:{message}");
            return State.Success;
        }
    }
    
}