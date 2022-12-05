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
                //Entity Has PointerToGameObject
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
                       enemyStateModel.TransparentCovers ??= new HashSet<Transform>();
                       enemyStateModel.TransparentCovers.Add(enter.collider.transform);
                   
                   }
                   else
                   {
                       enemyStateModel.Covers ??= new HashSet<Transform>();
                       enemyStateModel.Covers.Add(enter.collider.transform);
                   }
               }
               else
               {
                   if (enter.collider.gameObject.layer == LayerMask.NameToLayer(TRANPARENT_OBSTACLE_NAME))
                   {
                       enemyStateModel.UndefinedTransparentCovers ??= new HashSet<Transform>();
                       enemyStateModel.UndefinedTransparentCovers.Add(enter.collider.transform);
                   }
                   else
                   {
                       enemyStateModel.UndefinedCovers ??= new HashSet<Transform>();
                       enemyStateModel.UndefinedCovers.Add(enter.collider.transform);
                    
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
                //Entity Has PointerToGameObject
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
                       enemyStateModel.TransparentCovers.Remove(exit.collider.transform);
                   
                   }
                   else
                   {
                       enemyStateModel.Covers.Remove(exit.collider.transform);
                   }
               }
               else
               {
                   if (exit.collider.gameObject.layer == LayerMask.NameToLayer(TRANPARENT_OBSTACLE_NAME))
                   {
                       enemyStateModel.UndefinedTransparentCovers.Remove(exit.collider.transform);
                   
                   }
                   else
                   {
                       enemyStateModel.UndefinedCovers.Remove(exit.collider.transform);
                   }
               }
        
            }
            
        }
    }
}