using Ingame.Behaviour;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Enemy
{
    public class ReloadActionNode : ActionNode
    {
        [SerializeField] 
        private float timeToReload;

        private float _timer;
        protected override void ActOnStart()
        {
            _timer = timeToReload;
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

            ref var enemy = ref Entity.Get<EnemyStateModel>();
            enemy.CurrentAmmo = enemy.MaxAmmo;
            return State.Success;
        }
    }
}