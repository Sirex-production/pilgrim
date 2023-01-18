using Ingame.Behaviour;
using Leopotam.Ecs;
using UnityEngine;
namespace Ingame.Enemy
{
    public sealed class RandomSetPositionActionNode : ActionNode
    {
        [SerializeField]
        private Vector2 range;

        private float _minRange = 5f;
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
            
            ref var navAgent =ref entity.Get<NavMeshAgentModel>();
            var currentPosition = navAgent.Agent.transform.position;

            bool isMinRange = x * x + z * z > _minRange * _minRange;
            var newAddedPosition = new Vector3(x, 0, z);
            var newPosition = currentPosition + (isMinRange?newAddedPosition:newAddedPosition.normalized*_minRange);
            navAgent.Agent.destination = newPosition;
            return State.Success;
        }
    }
}