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
        
        [SerializeField]
        [ShowIf("IsLineCasting")]
        private LayerMask ignoredLayers;
        
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
                VisibilityType.LineCast => ActOnLineCasting(),
                VisibilityType.PhotoScanning => ActOnPhotoScanning(),
                _ => throw new InvalidOperationException()
            };
        }


        private State ActOnPhotoScanning()
        {
          

            ref var enemyModel = ref Entity.Get<EnemyStateModel>();
            return thresholdOfVisibility <= enemyModel.VisibleTagretPixels ? State.Success : State.Failure; 
        }
        
        private State ActOnLineCasting()
        {
            ref var transformModel = ref Entity.Get<TransformModel>();
            ref var enemyModel = ref Entity.Get<EnemyStateModel>();

            if (!Physics.Linecast(transformModel.transform.position, enemyModel.Target.position, out var hit,
                    ignoredLayers, QueryTriggerInteraction.Ignore)) return State.Success;

            if (!hit.collider.transform.root.TryGetComponent<EntityReference>(out var entityReference))
                return State.Failure;
            
            return entityReference.Entity.Has<PlayerModel>() ? State.Success : State.Failure;
        }
        
        private bool IsPhotoScanning() => type == VisibilityType.PhotoScanning;
        private bool IsLineCasting() => type == VisibilityType.LineCast;
    }
}