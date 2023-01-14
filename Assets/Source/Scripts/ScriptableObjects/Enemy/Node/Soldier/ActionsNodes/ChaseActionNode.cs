using Leopotam.Ecs;
using NaughtyAttributes;
using UnityEngine;

namespace Ingame.Enemy
{
    public class ChaseActionNode : RepositionActionNode
    {
        [SerializeField] 
        private TypeOfDetection typeOfDetection;
        
        [SerializeField] 
        [ShowIf("IsPhotoScanningDetectionUsed")]
        [Min(0)]
        private int visibility;
        
        [SerializeField]
        [Min(0)]
        private float distance;
        
        protected override State ActOnTick()
        {
            ref var enemyModel = ref Entity.Get<EnemyStateModel>();
            ref var agentModel = ref  Entity.Get<NavMeshAgentModel>();
            agentModel.Agent.destination = enemyModel.target.position;

            if (typeOfDetection == TypeOfDetection.PhotoScanning && enemyModel.visibleTargetPixels >= visibility)
                return State.Success;
            
            if (typeOfDetection == TypeOfDetection.RayCast && enemyModel.isTargetVisible)
                return State.Success;
            
            if (Vector3.Distance(enemyModel.target.position, agentModel.Agent.transform.position)<= distance)
            {
                return State.Success;
            }
            
            return base.ActOnTick();
        }
        private bool IsPhotoScanningDetectionUsed() => typeOfDetection == TypeOfDetection.PhotoScanning;
    }
}