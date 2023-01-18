using Ingame.Behaviour;
using Ingame.Movement;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Enemy
{
    public class GenerateNoiseActionNode : ActionNode
    {
        protected override void ActOnStart()
        {
             
        }

        protected override void ActOnStop()
        {
             
        }

        protected override State ActOnTick()
        {
            ref var transformModel = ref entity.Get<TransformModel>();
            world.CreateNoiseEvent(transformModel.transform.position);
            return State.Success;
        }
    }
}