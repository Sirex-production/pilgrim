using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ingame.Behaviour;
using Ingame.Movement;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Enemy
{
    public class DetectNoiseActionNode : ActionNode
    {
        [SerializeField] private float noiseDetectionRange = 10;
        private Transform _position;
        protected override void ActOnStart()
        {
             _position = entity.Get<TransformModel>().transform;
        }

        protected override void ActOnStop()
        {
            
        }

        protected override State ActOnTick()
        {
            ref var enemy = ref entity.Get<EnemyStateModel>();
            enemy.lastRememberedNoises ??= new List<Vector3>();

            for (int i = enemy.lastRememberedNoises.Count-1; i >=0 ; i--)
            {
                if (Vector3.Distance(_position.position, enemy.lastRememberedNoises[i]) > noiseDetectionRange)
                {
                    enemy.lastRememberedNoises.RemoveAt(i);
                    continue;
                }

                if (!enemy.isTargetDetected)
                {
                    enemy.hasDetectedNoises = true;
                    enemy.noisePosition = enemy.lastRememberedNoises[i];
                }
                enemy.lastRememberedNoises.RemoveAt(i);
            }
            
            return State.Success;
        }
    }
}