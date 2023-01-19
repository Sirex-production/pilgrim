using System.Collections.Generic;
using Ingame.Cover;
using Ingame.Enemy;
using LeoEcsPhysics;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Systems
{
    public sealed class EnemyObstacleDetectionSystem : IEcsRunSystem
    {
        private const string TRANPARENT_OBSTACLE_NAME = "TransparentObstacle";
        
        private readonly EcsFilter<OnTriggerEnterEvent> _enterFilter;
        private readonly EcsFilter<OnTriggerExitEvent> _exitFilter;
        
        public void Run()
        {
            //enter filter
            foreach (var i in _enterFilter)
            {
                ref var enter = ref _enterFilter.Get1(i);
                //has entity Reference
                if (!enter.senderGameObject.TryGetComponent<EntityReference>(out var entityReference))
                {
                    continue;
                }
                //entity Has PointerToGameObject
                if(!entityReference.Entity.Has<PointerToParentGameObjectComponent>())
                    continue;
                
                ref var parent = ref entityReference.Entity.Get<PointerToParentGameObjectComponent>().Parent;
                if (!parent.TryGetComponent<EntityReference>(out var entityParentReference))
                {
                    continue;
                }

                if (!entityParentReference.Entity.Has<EnemyStateModel>())
                {
                    continue;
                }

                ref var enemyStateModel = ref entityParentReference.Entity.Get<EnemyStateModel>();
               if(!enter.collider.CompareTag("CoverPoint"))
                   continue;
               
               
               if (enter.collider.TryGetComponent<EntityReference>(out var coverEntityReference) && coverEntityReference.Entity.Has<CoverComponent>())
               {
                   if (enter.collider.gameObject.layer == LayerMask.NameToLayer(TRANPARENT_OBSTACLE_NAME))
                   {
                       enemyStateModel.transparentCovers ??= new HashSet<Transform>();
                       enemyStateModel.transparentCovers.Add(enter.collider.transform);
                   
                   }
                   else
                   {
                       enemyStateModel.covers ??= new HashSet<Transform>();
                       enemyStateModel.covers.Add(enter.collider.transform);
                   }
               }
               else
               {
                   if (enter.collider.gameObject.layer == LayerMask.NameToLayer(TRANPARENT_OBSTACLE_NAME))
                   {
                       enemyStateModel.undefinedTransparentCovers ??= new HashSet<Transform>();
                       enemyStateModel.undefinedTransparentCovers.Add(enter.collider.transform);
                   }
                   else
                   {
                       enemyStateModel.undefinedCovers ??= new HashSet<Transform>();
                       enemyStateModel.undefinedCovers.Add(enter.collider.transform);
                    
                   }
               }
        
            }
            
            
            //exit
            foreach (var i in _exitFilter)
            {
                ref var exit = ref _exitFilter.Get1(i);
                //has entity Reference
                if (!exit.senderGameObject.TryGetComponent<EntityReference>(out var entityReference))
                {
                    continue;
                }
                //entity Has PointerToGameObject
                if(!entityReference.Entity.Has<PointerToParentGameObjectComponent>())
                    continue;
                
                ref var parent = ref entityReference.Entity.Get<PointerToParentGameObjectComponent>().Parent;
                if (!parent.TryGetComponent<EntityReference>(out var entityParentReference))
                {
                    continue;
                }

                if (!entityParentReference.Entity.Has<EnemyStateModel>())
                {
                    continue;
                }

                ref var enemyStateModel = ref entityParentReference.Entity.Get<EnemyStateModel>();
               if(!exit.collider.CompareTag("CoverPoint"))
                   continue;
               
               
               if (exit.collider.TryGetComponent<EntityReference>(out var coverEntityReference) && coverEntityReference.Entity.Has<CoverComponent>())
               {
                   if (exit.collider.gameObject.layer == LayerMask.NameToLayer(TRANPARENT_OBSTACLE_NAME))
                   {
                       enemyStateModel.transparentCovers.Remove(exit.collider.transform);
                   
                   }
                   else
                   {
                       enemyStateModel.covers.Remove(exit.collider.transform);
                   }
               }
               else
               {
                   if (exit.collider.gameObject.layer == LayerMask.NameToLayer(TRANPARENT_OBSTACLE_NAME))
                   {
                       enemyStateModel.undefinedTransparentCovers.Remove(exit.collider.transform);
                   
                   }
                   else
                   {
                       enemyStateModel.undefinedCovers.Remove(exit.collider.transform);
                   }
               }
        
            }
            
        }
    }
}