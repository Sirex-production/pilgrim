using Ingame.Behaviour;
using Ingame.Movement;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Enemy
{
    public class RunAwayActionNode : ActionNode
    {
        private Transform _target;
        private Transform _transform;
        
        protected override void ActOnStart()
        {
            _target = entity.Get<EnemyStateModel>().target;
            _transform = entity.Get<TransformModel>().transform;
        }

        protected override void ActOnStop()
        {
        
        }

        protected override State ActOnTick()
        {

            return State.Success;
        }
    }
}