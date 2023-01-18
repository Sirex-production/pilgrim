using Ingame.Behaviour;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Enemy
{
    public class IsLowAmmoNode : ActionNode
    {
        [SerializeField]
        [Range(0,1)]
        private float minAmmoPercentage = 0f;
        protected override void ActOnStart()
        {
             
        }

        protected override void ActOnStop()
        {
           
        }

        protected override State ActOnTick()
        {
            ref var enemy = ref entity.Get<EnemyStateModel>();
            return enemy.currentAmmo <= minAmmoPercentage*enemy.maxAmmo? State.Success : State.Failure;
        }
    }
}