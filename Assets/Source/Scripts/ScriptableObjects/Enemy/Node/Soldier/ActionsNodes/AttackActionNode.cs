using System;
using Ingame.Behaviour;
using Ingame.Health;
using Ingame.Movement;
using Leopotam.Ecs;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Ingame.Enemy
{
    public class AttackActionNode : ActionNode
    {
        enum TypeOfAttack
        {
            UnfairAccuracy,
            ProgressiveAccuracy
        }

        [SerializeField] 
        private TypeOfAttack typeOfAttack;
        [SerializeField] 
        private float damageOnHit;
        [SerializeField] 
        [Range(0, 1)] 
        private float chanceToHit = 0.9f;
        [SerializeField] 
        private float shootIntervalTime = 1.5f;
        [SerializeField] 
        private bool shouldIgnoreObstacles;

        private float _minAccuracy = 0.15f;
        private float _accurencyRate = 0.045f;
        private float _currentIntervalTime;
      
        protected override void ActOnStart()
        {
            _currentIntervalTime = shootIntervalTime;
            Entity.Get<EnemyStateModel>().isAttacking = true;
        }

        protected override void ActOnStop()
        {
            Entity.Get<EnemyStateModel>().isAttacking = false;
        }
        /// <summary>
        /// Try to attack after [shootIntervalTime] time
        /// </summary>
        /// <returns>Return Success if hit target, Failure if not and Running if it's still on a cooldown</returns>
        protected override State ActOnTick()
        {
            ref var enemyModel = ref Entity.Get<EnemyStateModel>();

            //cooldown
            if (_currentIntervalTime>0)
            {
                _currentIntervalTime -= Time.deltaTime;
                return State.Running;
            }
            
            enemyModel.currentAmmo -= 1;
            
            //hit chance - do miss
            if (typeOfAttack == TypeOfAttack.UnfairAccuracy)
            {
                if (chanceToHit < Random.Range(0f, 1f))
                {
                    return State.Failure;
                }
            }

            if (typeOfAttack == TypeOfAttack.ProgressiveAccuracy)
            {
                float chanceToHit = enemyModel.visibleTargetPixels * _accurencyRate;
                float totalAcc = Math.Clamp(chanceToHit,_minAccuracy,this.chanceToHit);
                if (totalAcc < Random.Range(0f, 1f))
                {
                    return State.Failure;
                }
            }
            
            if (!enemyModel.target.TryGetComponent(out EntityReference targetEntityReference))
                return State.Failure;
            targetEntityReference.Entity.Get<HealthComponent>().currentHealth -= damageOnHit;
            return State.Success;

        }
    }
}