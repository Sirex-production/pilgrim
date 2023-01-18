using System.Collections;
using System.Collections.Generic;
using Ingame.Enemy;
using Ingame.Movement;
using Ingame.Player;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Behaviour
{
    public class BehaviourBinderSystem : IEcsRunSystem
    {
        private readonly EcsFilter<BehaviourAgentModel,BehaviourBinderRequest> _filter;
        private readonly EcsFilter<PlayerModel,TransformModel> _filterPlayer;

        public void Run()
        {
            if (_filter.IsEmpty())
                return;
            
            if(_filterPlayer.IsEmpty())
                    return;

            var player = _filterPlayer.Get2(0);
   
            //Bind Entity With tree
            foreach (var i in _filter)
            {
                
                ref var entity = ref _filter.GetEntity(i);
                _filter.Get1(i).tree = _filter.Get1(i).OriginalTree.Clone();
                _filter.Get1(i).tree.Entity = entity;

                //Bind Player With EnemyStateModel
                if (entity.Has<EnemyStateModel>())
                {
                    entity.Get<EnemyStateModel>().target = player.transform;
                }
                
                entity.Get<BehaviourCheckerTag>();
                entity.Del<BehaviourBinderRequest>();
            }
        }
    }
}