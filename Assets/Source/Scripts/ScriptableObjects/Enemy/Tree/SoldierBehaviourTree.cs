using Ingame.Enemy;
using Ingame.Health;
using Ingame.Movement;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Behaviour
{
    [CreateAssetMenu(fileName = "SoldierBehaviourTree",menuName = "Behaviour/Soldier/Tree")]
    public sealed class SoldierBehaviourTree : BehaviourTree
    {
        public override bool Init()
        {
            return IsSoldierEntity(ref Entity);
        }

        private bool IsSoldierEntity(ref EcsEntity entity)
        {
            return entity.Has<NavMeshAgentModel>()&&
                   entity.Has<WayPointsComponent>()&&
                   entity.Has<HealthComponent>()&&
                   entity.Has<TransformModel>()&&
                   entity.Has<EnemyStateModel>()&&
                   entity.Has<AnimatorModel>();
        }
    }
}