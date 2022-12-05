using System.Timers;
using Ingame.Behaviour;
using Ingame.Health;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Enemy
{
    public class HealActionNode : ActionNode
    {
        [SerializeField] private float timeToHeal = 2f;
        [SerializeField] 
        [Range(0, 1)] 
        private float percentageOfHeal = 0.4f;
        private float _timer;
        protected override void ActOnStart()
        {
            _timer = timeToHeal;
        }

        protected override void ActOnStop()
        {
        
        }

        protected override State ActOnTick()
        {
            if (_timer > 0)
            {
                _timer -= Time.deltaTime;
                return State.Running;
            }
            ref var health = ref Entity.Get<HealthComponent>();
            health.currentHealth += percentageOfHeal * health.initialHealth;
            return State.Success;
        }
    }
}