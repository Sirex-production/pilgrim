using System;
using System.Collections.Generic;
using System.IO;
using Ingame.Behaviour;
using Ingame.Cover;
using Leopotam.Ecs;
using ModestTree;
using UnityEngine;
using UnityEngine.AI;

namespace Ingame.Enemy
{
    public enum TypeOfCover
    {
        Solid,
        Transparent,
        Both
    }

   
    public class FindCoverPointActionNode : ActionNode
    {
    
        
        private enum TypeOfCoverSpot
        {
            Dynamic,
            Fixed,
            Both
        }

        [SerializeField] 
        private TypeOfCover typeOfCover;
        [SerializeField] 
        private TypeOfCoverSpot typeOfCoverSpot;

        private NavMeshPath _shortestPath ;
        private float _minDistance;
        
        protected override void ActOnStart()
        {
            _minDistance = float.MaxValue;
        }

        protected override void ActOnStop()
        {
             
        }
        private void OnEnable()
        {
            _shortestPath = new NavMeshPath();
        }
        
        protected override State ActOnTick()
        {
            ref var navAgent = ref Entity.Get<NavMeshAgentModel>();
            ref var enemy = ref Entity.Get<EnemyStateModel>();
            

            switch ((typeOfCoverSpot,typeOfCover))
            {
                case (TypeOfCoverSpot.Dynamic,TypeOfCover.Solid):
                    ComparePaths(ref enemy,navAgent.Agent,enemy.UndefinedCovers,false);
                    break;
                
                case (TypeOfCoverSpot.Dynamic,TypeOfCover.Transparent):
                    ComparePaths(ref enemy,navAgent.Agent,enemy.UndefinedTransparentCovers,false);
                    break;
                
                case (TypeOfCoverSpot.Fixed,TypeOfCover.Solid):
                    ComparePaths(ref enemy,navAgent.Agent,enemy.Covers,true);
                    break;
                
                case (TypeOfCoverSpot.Fixed,TypeOfCover.Transparent):
                    ComparePaths(ref enemy,navAgent.Agent,enemy.TransparentCovers,true);
                    break;
                //mixed
                case (TypeOfCoverSpot.Both,TypeOfCover.Solid):
                    ComparePaths(ref enemy,navAgent.Agent,enemy.Covers,true);
                    ComparePaths(ref enemy,navAgent.Agent,enemy.UndefinedCovers,false);
                    break;
                
                case (TypeOfCoverSpot.Both,TypeOfCover.Transparent):
                    ComparePaths(ref enemy,navAgent.Agent,enemy.UndefinedTransparentCovers,false);
                    ComparePaths(ref enemy,navAgent.Agent,enemy.TransparentCovers,true);
                    break;
                
                case (TypeOfCoverSpot.Fixed,TypeOfCover.Both):
                    ComparePaths(ref enemy,navAgent.Agent,enemy.TransparentCovers,true);
                    ComparePaths(ref enemy,navAgent.Agent,enemy.Covers,true);
                    break;
                
                case (TypeOfCoverSpot.Dynamic,TypeOfCover.Both):
                    ComparePaths(ref enemy,navAgent.Agent,enemy.UndefinedTransparentCovers,false);
                    ComparePaths(ref enemy,navAgent.Agent,enemy.UndefinedCovers,false);
                    break;
                
                case (TypeOfCoverSpot.Both,TypeOfCover.Both):
                    ComparePaths(ref enemy,navAgent.Agent,enemy.UndefinedCovers,false);
                    ComparePaths(ref enemy,navAgent.Agent,enemy.UndefinedTransparentCovers,false);
                    ComparePaths(ref enemy,navAgent.Agent,enemy.Covers,true);
                    ComparePaths(ref enemy,navAgent.Agent,enemy.TransparentCovers,true);
                    break;
            }
            //checks if cover is determined
            if (_minDistance>=float.MaxValue)
            {
                return State.Failure;
            }

            navAgent.Agent.SetPath(_shortestPath);
            return State.Success;
        }

        private void ComparePaths(ref EnemyStateModel enemy, NavMeshAgent navAgent,HashSet<Transform> transforms,bool hasDeterminedPoints)
        {
            var shortestDistance = GetShortestPath(transforms, navAgent, enemy.Target,hasDeterminedPoints);
            if (shortestDistance.Item1 && _minDistance> shortestDistance.Item2)
            {
                _shortestPath = shortestDistance.Item4;
                _minDistance = shortestDistance.Item2;
            }
        }
        
        private (bool,float, Vector3,NavMeshPath) GetShortestPath(HashSet<Transform> transforms, NavMeshAgent agent, Transform target,bool hasDeterminedPoints)
        {
            if (transforms==null||transforms.IsEmpty())
            {
                return (false, float.MaxValue, Vector3.zero, null);
            }
            var min = float.MaxValue;
            var shortestPath = new NavMeshPath();
            Vector3 point = agent.transform.position;
            var isShortestPathFound = false;
            
            foreach (var transform in transforms)
            {
                var points = new List<Vector3>();
                //undefined cover
                if (!hasDeterminedPoints)
                {
                    //var size = transform.GetComponent<Renderer>().bounds.size
                    points.Add(transform.position+(transform.position-target.position).normalized);
                }
                //defined cover
                else
                {
                    if (!transform.TryGetComponent<EntityReference>(out var entityRef) && !entityRef.Entity.Has<CoverComponent>())
                    {
                        continue;
                    }
                    ref var cover = ref entityRef.Entity.Get<CoverComponent>();
                    for (var i = 0; i < cover.CoverPoints.Count; i++)
                    {
                        var coverPoint = cover.CoverPoints[i].position;
                        if (!Physics.Raycast(coverPoint, (target.position-coverPoint).normalized, out var hit) || hit.collider.transform != transform)
                        {
                            continue;
                        }
                        points.Add(coverPoint);
                    }
                }
                //checks cover points
                foreach (var p in points)
                {
                    var path = new NavMeshPath();
                    //determine a path
                    if (!NavMesh.CalculatePath(agent.transform.position, p,agent.areaMask, path))
                    {
                        continue;
                    }
                    //calculate a real distance
                    var dist = Vector3.Distance(transform.position, path.corners[0]);
                    for (var j = 1; j < path.corners.Length; j++)
                    {
                        dist += Vector3.Distance(path.corners[j - 1], path.corners[j]);
                    }
                    //determine if path is the closest
                    if (!(dist < min)) continue;
                    min = dist;
                    point = transform.position;
                    shortestPath = path;
                    isShortestPathFound = true;
                }
            }
            
            return (isShortestPathFound,min, point,shortestPath);
        }
        
      
    }
}