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
             _position = Entity.Get<TransformModel>().transform;
        }

        protected override void ActOnStop()
        {
            
        }

        protected override State ActOnTick()
        {
            ref var enemy = ref Entity.Get<EnemyStateModel>();
            enemy.LastRememberedNoises ??= new List<Vector3>();

            for (int i = enemy.LastRememberedNoises.Count-1; i >=0 ; i--)
            {
                if (Vector3.Distance(_position.position, enemy.LastRememberedNoises[i]) > noiseDetectionRange)
                {
                    enemy.LastRememberedNoises.RemoveAt(i);
                    continue;
                }

                if (!enemy.IsTargetDetected)
                {
                    enemy.HasDetectedNoises = true;
                    enemy.NoisePosition = enemy.LastRememberedNoises[i];
                }
                enemy.LastRememberedNoises.RemoveAt(i);
            }
           
           
            //Noise Detection
            return State.Success;
        }
    }
}