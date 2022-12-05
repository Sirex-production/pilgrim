using System.Collections;
using System.Collections.Generic;
using Ingame.Enemy;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Behaviour
{
    public class BehaviourBinderSystem : IEcsRunSystem
    {
        private readonly EcsFilter<BehaviourAgentModel,BehaviourBinderRequest> _filter;

        public void Run()
        {
            if (_filter.IsEmpty())
            {
                return;
            }
            var player = GameObject.FindGameObjectWithTag("Player");
   
            //Bind Entity With Tree
            foreach (var i in _filter)
            {
                
                ref var entity = ref _filter.GetEntity(i);
                _filter.Get1(i).Tree.Entity = entity;

                //Bind Player With EnemyStateModel
                if (entity.Has<EnemyStateModel>())
                {
                    entity.Get<EnemyStateModel>().Target = player.transform;
                }
                
                entity.Get<BehaviourCheckerTag>();
                entity.Del<BehaviourBinderRequest>();
            }
        }
    }
}