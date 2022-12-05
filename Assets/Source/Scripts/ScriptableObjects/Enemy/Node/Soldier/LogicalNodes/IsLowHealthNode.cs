using Ingame.Behaviour;
using Ingame.Health;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Enemy
{
    public sealed class IsLowHealthNode : ActionNode
    {
        [SerializeField]
        [Range(0,1)]
        private float healthThreshold;
        
        protected override void ActOnStart()
        {
          
        }

        protected override void ActOnStop()
        {
           
        }

        protected override State ActOnTick()
        {
            ref var health = ref Entity.Get<HealthComponent>();
            if (health.currentHealth<=0)
            {
                Entity.Get<EnemyStateModel>().IsDying = true;
            }
            return health.currentHealth > healthThreshold * health.initialHealth ? State.Failure : State.Success;
        }
    }
}