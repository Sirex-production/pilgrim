using Ingame.Behaviour;
using Leopotam.Ecs;
using UnityEngine;
namespace Ingame.Enemy
{
    public sealed class RandomSetPositionActionNode : ActionNode
    {
        [SerializeField]
        private Vector2 range;
        protected override void ActOnStart()
        {
             
        }

        protected override void ActOnStop()
        {
         
        }

        protected override State ActOnTick()
        {
            var x = Random.Range(-range.x,range.x);
            var z = Random.Range(-range.y,range.y);

            ref var navAgent =ref Entity.Get<NavMeshAgentModel>();
            var currentPosition = navAgent.Agent.transform.position;

            var newPosition = currentPosition + new Vector3(x, 0, z);
            navAgent.Agent.destination = newPosition;
            return State.Success;
        }
    }
}