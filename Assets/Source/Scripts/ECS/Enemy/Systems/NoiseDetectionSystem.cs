using System;
using System.Collections.Generic;
using Ingame.Enemy;
using Ingame.Movement;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Systems
{
    public class NoiseDetectionSystem : IEcsRunSystem
    {
        private readonly EcsFilter<NoiseTagRequest> _noiseFilter;
        private readonly EcsFilter<EnemyStateModel> _enemyFilter;
        public void Run()
        {
            foreach (var i in _noiseFilter)
            {
                ref var noise = ref _noiseFilter.Get1(i);
                foreach (var j in _enemyFilter)
                {
                    ref var enemy = ref _enemyFilter.Get1(j);
                    if (enemy.IsTargetDetected) continue;
                    enemy.LastRememberedNoises ??= new List<Vector3>();
                    enemy.LastRememberedNoises.Add(noise.Position);
                }
                _noiseFilter.GetEntity(i).Destroy();
            }
        }
    }
}