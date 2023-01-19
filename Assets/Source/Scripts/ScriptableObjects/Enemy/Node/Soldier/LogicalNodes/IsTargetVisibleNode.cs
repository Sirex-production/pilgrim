using System;
using Ingame.Behaviour;
using Ingame.Movement;
using Ingame.Player;
using Leopotam.Ecs;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ingame.Enemy
{
    public class IsTargetVisibleNode : ActionNode
    {
        private enum VisibilityType
        {
            PhotoScanning,
            LineCast
        }

        [SerializeField] private VisibilityType type;
        
        
        [SerializeField] 
        [Min(0)]
        [ShowIf("IsPhotoScanning")]
        private int thresholdOfVisibility;
        
        protected override void ActOnStart()
        {
           
        }

        protected override void ActOnStop()
        {
            
        }

        protected override State ActOnTick()
        {
            return type switch
            {
                VisibilityType.LineCast => ActOnRayeCasting(),
                VisibilityType.PhotoScanning => ActOnPhotoScanning(),
                _ => throw new InvalidOperationException()
            };
        }


        private State ActOnPhotoScanning()
        {
            return thresholdOfVisibility <= entity.Get<EnemyStateModel>().visibleTargetPixels ? State.Success : State.Failure; 
        }

        private State ActOnRayeCasting()
        {
            return entity.Get<EnemyStateModel>().isTargetVisible ? State.Success : State.Failure; 
        }
       
        private bool IsPhotoScanning() => type == VisibilityType.PhotoScanning;
    }
}