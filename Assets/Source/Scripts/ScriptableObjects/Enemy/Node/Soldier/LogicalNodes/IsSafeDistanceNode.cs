using System;
using Ingame.Behaviour;
using Ingame.Movement;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Enemy
{
    public class IsSafeDistanceNode : ActionNode
    {
        private enum DistanceType
        {
            Greater,
            Smaller
        }
        [SerializeField] private float distance;
        [SerializeField] private DistanceType type;
        protected override void ActOnStart()
        {
             
        }

        protected override void ActOnStop()
        {
            
        }

        protected override State ActOnTick()
        {
            ref var target = ref Entity.Get<EnemyStateModel>().Target;
            ref var that = ref Entity.Get<TransformModel>().transform;
            if (target==null)
            {
                #if UNITY_EDITOR
                    UnityEngine.Debug.LogWarning($"The target is null in Node{this.name.ToString()} for Entity {this.Entity.ToString()}");
                #endif
                return State.Failure;
            }

            return type switch
            {
                DistanceType.Greater => Vector3.Distance(target.position, that.position) >= distance ? State.Success : State.Failure,
                DistanceType.Smaller => Vector3.Distance(target.position, that.position) < distance ? State.Success : State.Failure,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}