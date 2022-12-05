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
            ref var enemy = ref Entity.Get<EnemyStateModel>();
            return enemy.CurrentAmmo <= minAmmoPercentage*enemy.MaxAmmo? State.Success : State.Failure;
        }
    }
}