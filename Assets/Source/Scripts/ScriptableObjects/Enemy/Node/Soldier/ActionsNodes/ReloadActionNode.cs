using Ingame.Behaviour;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Enemy
{
    public class ReloadActionNode : WaitNode
    {
        protected override void ActOnStart()
        {
            base.ActOnStart();
            Entity.Get<EnemyStateModel>().isReloading = true;
        }

        protected override void ActOnStop()
        {
            base.ActOnStop();
            ref var enemyModel = ref Entity.Get<EnemyStateModel>();
            enemyModel.isReloading = false;
            enemyModel.currentAmmo = enemyModel.maxAmmo;
        }
        
    }
}