using LeoEcsPhysics;
using UnityEngine;
using Voody.UniLeo;

namespace Ingame.Enemy
{
    [RequireComponent( typeof(OnTriggerEnterChecker),typeof(PointerToParentGameObjectComponentProvider),typeof(OnTriggerExitChecker))]
    public class EnemyObstacleDetectorTagProvider : MonoProvider<EnemyObstacleDetectorTag>
    {
        
    }
}