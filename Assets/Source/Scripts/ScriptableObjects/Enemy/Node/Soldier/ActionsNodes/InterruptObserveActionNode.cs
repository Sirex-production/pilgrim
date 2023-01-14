using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Enemy
{
    public sealed class InterruptObserveActionNode : ObserveActionNode
    {
        [SerializeField] 
        private TypeOfDetection typeOfDetection = TypeOfDetection.PhotoScanning;
        
        
        protected override State ActOnTick()
        {
            var state = base.ActOnTick();
            
            ref var enemyModel = ref Entity.Get<EnemyStateModel>();
            
            
            if (typeOfDetection == TypeOfDetection.PhotoScanning)
            {
                return enemyModel.visibleTargetPixels > 0 ? State.Success : state;
            }
            
            if (typeOfDetection == TypeOfDetection.RayCast)
            {
                return enemyModel.isTargetVisible ? State.Success : state;
            }
            
            return state;
        }
    }
}